using LibraryManagement.Api;
using LibraryManagement.Contract;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure;
using LibraryManagement.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Fixtures;

public class SqliteTestDatabaseFixture : IAsyncLifetime
{
    public Container Container { get; private set; } = null!;
    private SqliteConnection _sqliteConnection = null!;

    public async Task InitializeAsync()
    {
        Container = new Container();

        Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        _sqliteConnection = new SqliteConnection("DataSource=:memory:");
        await _sqliteConnection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryManagementDbContext>()
            .UseSqlite(_sqliteConnection)
            .Options;

        Container.AddAutoMapper();
        Container.Register(typeof(ILogger<>), typeof(Logger<>), Lifestyle.Singleton);
        Container.AddInfrastructure(options);
        Container.AddApplication();
        Container.RegisterSingleton<ILoggerFactory>(() =>
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
        });

        using (AsyncScopedLifestyle.BeginScope(Container))
        {
            var context = Container.GetInstance<LibraryManagementDbContext>();
            await context.Database.EnsureCreatedAsync();
            await SeedTestDataAsync(context);
        }
        Container.Verify();
    }

    public async Task SeedTestDataAsync(LibraryManagementDbContext context)
    {
        context.Authors.AddRange(GetTestAuthors());
        await context.SaveChangesAsync();

        context.Categories.AddRange(GetTestCategories());
        await context.SaveChangesAsync();

        context.Books.AddRange(GetTestBooks());
        await context.SaveChangesAsync();

        context.Users.AddRange(GetTestUsers());
        await context.SaveChangesAsync();

        context.Borrowings.AddRange(GetTestBorrowings());
        await context.SaveChangesAsync();
    }

    public List<Author> GetTestAuthors() => new()
    {
        new Author
        {
            FirstName = "Test Name 1",
            LastName = "Last Name 1",
            Biography = "Test Biography 1",
            DateOfBirth = new DateTime(1950,01,01),
            IsActive = true
        },
        new Author
        {
            FirstName = "Test Name 2",
            LastName = "Last Name 2",
            Biography = "Test Biography 2",
            DateOfBirth = new DateTime(1970,01,01),
            IsActive = true
        },
        new Author
        {
            FirstName = "Test Name 3",
            LastName = "Last Name 3",
            Biography = "Test Biography 3",
            DateOfBirth = new DateTime(1973,03,03),
            IsActive = true
        }
    };

    public List<Book> GetTestBooks() => new()
    {
        new Book
        {
            Title = "Test Book 1",
            ISBN = "1111111111111",
            Description = "Test Description 1",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = new DateTime(2000,01,01),
            PageCount = 100,
            IsAvailable = true,
            CreatedDate = new DateTime(2020,01,01),
            UpdatedDate = new DateTime(2022,01,01),
        },
        new Book
        {
            Title = "Test Book 2",
            ISBN = "2222222222222",
            Description = "Test Description 2",
            AuthorId = 2,
            CategoryId = 2,
            PublishedDate = new DateTime(2002,02,02),
            PageCount = 200,
            IsAvailable = false,
            CreatedDate= new DateTime(2022,02,02)
        },
        new Book
        {
            Title = "Test Book 3",
            ISBN = "3333333333333",
            Description = "Test Description 3",
            AuthorId = 2,
            CategoryId = 3,
            PublishedDate = new DateTime(2003,03,03),
            PageCount = 300,
            IsAvailable = true,
            CreatedDate= new DateTime(2023,03,03)
        }
    };

    public List<Borrowing> GetTestBorrowings() => new()
    {
        new Borrowing
        {
            BookId = 1,
            UserId = 1,
            BorrowDate = new DateTime(2025,01,01),
            DueDate = new DateTime(2025,02,01),
            ReturnDate = new DateTime(2025,03,01),
            Status = BorrowingStatus.Returned
        },
        new Borrowing
        {
            BookId = 2,
            UserId = 2,
            BorrowDate = new DateTime(2025,11,12),
            DueDate = new DateTime(2025,11,22),
            Status = BorrowingStatus.Overdue
        }
    };

    public List<Category> GetTestCategories() => new()
    {
        new Category
        {
            Name = "Test Category 1",
            Description = "Test Description 1",
            SortOrder = 1,
            IsActive = true
        },
        new Category
        {
            Name = "Test Category 2",
            Description = "Test Description 2",
            SortOrder = 1,
            IsActive = true
        },
        new Category
        {
            Name = "Test Category 3",
            Description = "Test Description 3",
            SortOrder = 1,
            IsActive = true
        }
    };

    public List<User> GetTestUsers() => new()
    {
        new User
        {
            UserId = 1
        },
        new User
        {
            UserId = 2
        },
        new User
        {
            UserId = 3
        }
    };

    public async Task DisposeAsync()
    {
        await _sqliteConnection.DisposeAsync();
        Container.Dispose();
    }
}
