using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared;
using Microsoft.EntityFrameworkCore;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Infrastructure;

public class BookRepositoryTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public BookRepositoryTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();

            Book? book = await _bookRepository.GetByIdAsync(1);

            Assert.NotNull(book);
            Assert.Equal(1, book.BookId);
            Assert.Equal("1111111111111", book.ISBN);
            Assert.Equal("Test Description 1", book.Description);
            Assert.Equal(1, book.AuthorId);
        }
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdDoesnotExist_ShouldReturnNull()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();

            Book? book = await _bookRepository.GetByIdAsync(111);
            Assert.Null(book);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            Book book = new Book
            {
            Title = "Test Book 4",
            ISBN = "4444444444444",
            Description = "Test Description 4",
            AuthorId = 2,
            CategoryId = 2,
            PublishedDate = new DateTime(2004,04,04),
            PageCount = 400,
            IsAvailable = true,
            CreatedDate = new DateTime(2024,04,04)
            };

            Book? addedBook = await _bookRepository.AddAsync(book);
            
            Assert.NotNull(addedBook);
            Assert.Equal(4, addedBook.BookId);
            Assert.Equal("4444444444444", addedBook.ISBN);
            Assert.Equal("Test Description 4", addedBook.Description);
            Assert.Equal("Test Book 4", addedBook.Title);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Book book = new Book
            {
                Title = "Test Book 3",
                ISBN = "1111111111111",
                Description = "Test Description 3",
                AuthorId = 3,
                CategoryId = 2,
                PublishedDate = new DateTime(2003, 03, 03),
                PageCount = 300,
                IsAvailable = true,
                CreatedDate = new DateTime(2020, 03, 03)
            };

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _bookRepository.AddAsync(book);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task UpdateAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Book book = new Book
            {
                BookId = 1,
                Title = "Test Book 4",
                ISBN = "1111111111111",
                Description = "Test Description 4",
                AuthorId = 1,
                CategoryId = 2,
                PublishedDate = new DateTime(2004, 04, 04),
                PageCount = 400,
                IsAvailable = true,
            };

            await _bookRepository.UpdateAsync(book);
            await context.SaveChangesAsync();

            var updatedBook = await _bookRepository.GetByIdAsync(1);

            Assert.NotNull(updatedBook);
            Assert.Equal(1, updatedBook.BookId);
            Assert.Equal("1111111111111", updatedBook.ISBN);
            Assert.Equal("Test Description 4", updatedBook.Description);
            Assert.Equal("Test Book 4", updatedBook.Title);
        }
    }

    [Fact]
    public async Task UpdateAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Book book = new Book
            {
                BookId = 1,
                Title = "Test Book 3",
                Description = "Test Description 4",
                AuthorId = 1,
                CategoryId = 2,
                PublishedDate = new DateTime(2003, 03, 03),
                PageCount = 300,
                IsAvailable = true,
                CreatedDate = new DateTime(2020, 03, 03)
            };

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _bookRepository.UpdateAsync(book);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task DeleteAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Book book = new Book
            {
                BookId = 3,
                Title = "Test Book 3",
                ISBN = "3333333333333",
                Description = "Test Description 3",
                AuthorId = 3,
                CategoryId = 3,
                PublishedDate = new DateTime(2003, 03, 03),
                PageCount = 300,
                IsAvailable = true,
                CreatedDate = new DateTime(2023, 03, 03)
            };

            await _bookRepository.DeleteAsync(book);
            await context.SaveChangesAsync();

            var deletedBook = await _bookRepository.GetByIdAsync(3);

            Assert.Null(deletedBook);
        }
    }

    [Fact]
    public async Task DeleteAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Book book = new Book
            {
                BookId = 4,
                Title = "Test Book 4",
                Description = "Test Description 4",
                AuthorId = 3,
                CategoryId = 3,
                PublishedDate = new DateTime(2003, 03, 03),
                PageCount = 300,
                IsAvailable = true,
                CreatedDate = new DateTime(2023, 03, 03)
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await _bookRepository.DeleteAsync(book);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task GetAllAsync_WhenIdsExist_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();

            ICollection<Book> books = await _bookRepository.GetAllAsync();

            Assert.NotNull(books);
            Assert.Equal(3, books.Count);
        }
    }

    [Fact]
    public async Task FindAsync_WhenEntityExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            BookSearchArgs args = new BookSearchArgs
            {
                AuthorId = 1,
                CategoryId = 1,
                PageNumber = 1,
                PageSize = 15
            };

            PagedResult<Book>? results = await _bookRepository.FindAsync(args);

            Assert.NotNull(results);
            Assert.Equal("Test Book 1", results.Items.First().Title);
        }
    }

    [Fact]
    public async Task FindAsync_WhenEntityDoesnotExist_ShouldReturnEmptyItems()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookRepository _bookRepository = _fixture.Container.GetInstance<IBookRepository>();
            BookSearchArgs args = new BookSearchArgs();

            PagedResult<Book>? results = await _bookRepository.FindAsync(args);

            Assert.NotNull(results);
            Assert.Equal([], results.Items);
        }
    }
}
