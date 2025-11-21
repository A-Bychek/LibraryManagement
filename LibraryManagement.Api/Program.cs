using LibraryManagement.Api;
using LibraryManagement.Api.Services;
using LibraryManagement.Application;
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

container.Register(typeof(ILogger<>), typeof(Logger<>), Lifestyle.Singleton);

builder.Services.AddSimpleInjector(container, options =>
{
    options.AddAspNetCore();
});

var options = new DbContextOptionsBuilder<LibraryManagementDbContext>()
  .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
  .Options;

container.AddAutoMapper();
container.AddApplication();
container.AddInfrastructure(options);
container.Register<GrpcAuthorService>(Lifestyle.Scoped);
container.Register<GrpcBookService>(Lifestyle.Scoped);

builder.Services.AddScoped<GrpcAuthorService>(sp => container.GetInstance<GrpcAuthorService>());
builder.Services.AddScoped<GrpcBookService>(sp => container.GetInstance<GrpcBookService>());

builder.Services.AddGrpc();

var app = builder.Build();

app.Services.UseSimpleInjector(container);

app.MapGrpcService<GrpcAuthorService>();
app.MapGrpcService<GrpcBookService>();

container.Verify();

app.MapGet("/", () => $"Library Management Service is running - {DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}");

app.Run();

Log.Information("The application has completed its work. Close the logger.");
Log.CloseAndFlush();
