using LibraryManagement.Api;
using LibraryManagement.Api.Interceptors;
using LibraryManagement.Api.Services;
using LibraryManagement.Contract;
using LibraryManagement.Infrastructure;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SimpleInjector;
using SimpleInjector.Lifestyles;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File($"logs/log_{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var container = new Container();

container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

builder.Services.AddSimpleInjector(container, options =>
{
    options.AddAspNetCore();
});

builder.Services.AddDbContext<LibraryManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
var options = new DbContextOptionsBuilder<LibraryManagementDbContext>()
    .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
    .Options;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

container.AddAutoMapper();
container.AddApplication();
container.AddInfrastructure(options);
container.Register<GrpcAuthorService>(Lifestyle.Scoped);
container.Register<GrpcBookService>(Lifestyle.Scoped);
container.Register<GrpcBorrowingService>(Lifestyle.Scoped);
container.Register<GrpcCategoryService>(Lifestyle.Scoped);

builder.Services.AddScoped<GrpcAuthorService>(sp => container.GetInstance<GrpcAuthorService>());
builder.Services.AddScoped<GrpcBookService>(sp => container.GetInstance<GrpcBookService>());
builder.Services.AddScoped<GrpcBorrowingService>(sp => container.GetInstance<GrpcBorrowingService>());
builder.Services.AddScoped<GrpcCategoryService>(sp => container.GetInstance<GrpcCategoryService>());

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowAnyOrigin()
                                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding",
                                "Grpc-Accept-Encoding", "Grpc-Status-Details-Bin")
                                .DisallowCredentials();
                      });
});

builder.Services.AddGrpc(options =>
    {
    options.Interceptors.Add<ExceptionHandlingInterceptor>();
    });

var app = builder.Build();
app.UseRouting();
app.UseGrpcWeb();

app.Services.UseSimpleInjector(container);

app.MapGrpcService<GrpcAuthorService>().EnableGrpcWeb();
app.MapGrpcService<GrpcBookService>().EnableGrpcWeb();
app.MapGrpcService<GrpcBorrowingService>().EnableGrpcWeb();
app.MapGrpcService<GrpcCategoryService>().EnableGrpcWeb();
app.UseCors("MyAllowSpecificOrigins");

container.Verify();

app.MapGet("/", () => $"Library Management Service is running - {DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}");

app.Run();

Log.Information("The application has completed its work. Close the logger.");
Log.CloseAndFlush();
