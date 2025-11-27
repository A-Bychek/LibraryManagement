using FluentValidation;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Application;

public class BookServiceTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public BookServiceTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetBookAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();

            BookDto? book = await _bookService.GetBookAsync(1);

            Assert.NotNull(book);
            Assert.Equal(1, book.AuthorId);
            Assert.Equal("1111111111111", book.ISBN);
            Assert.Equal("Test Title Updated", book.Title);
            Assert.Equal("Test Name 1 Last Name 1", book.AuthorName);
        }
    }
    
    [Fact]
    public async Task GetBookAsync_WhenIdDoesnotExist_ShouldThrowsNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();

            await Assert.ThrowsAsync<NotFoundException>(async () => await _bookService.GetBookAsync(111));
        }
    }
    
    [Fact]
    public async Task GetBooksAsync_IfRequestIsValid_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();
            BookSearchArgs args = new BookSearchArgs
            {
                SearchTerm = "Test",
                PageNumber = 1,
                PageSize = 15
            };

            PagedResult<BookDto>? books = await _bookService.GetBooksAsync(args);

            Assert.NotNull(books);
            Assert.Equal(3, books.TotalCount);

        }
    }

    [Fact]
    public async Task CreateBookAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();
            CreateBookCommand createBookCommand = new CreateBookCommand
            {
                Title = "Test Title 4",
                ISBN = "4444444444444",
                Description = "Test Description 4",
                AuthorId = 2,
                CategoryId = 2,
                PublishedDate = "2025-01-01",
                PageCount = 400

            };

            BookDto? addedBook = await _bookService.CreateBookAsync(createBookCommand);

            Assert.NotNull(addedBook);
            Assert.Equal(4, addedBook.BookId);
            Assert.Equal("Test Name 2 Last Name 2", addedBook.AuthorName);
            Assert.Equal(2, addedBook.AuthorId);
        }
    }

    [Fact]
    public async Task CreateBookAsync_IfRequestIsInvalid_ShouldThrowValidationException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();
            CreateBookCommand createBookCommand = new CreateBookCommand();

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _bookService.CreateBookAsync(createBookCommand);
            });
        }
    }

    [Fact]
    public async Task UpdateBookAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();
            UpdateBookCommand updateBookCommand = new UpdateBookCommand
            {
                BookId = 1,
                Title = "Test Title Updated",
                Description = "Test Description Updated",
                PublishedDate = "2020-01-01"
            };

            await _bookService.UpdateBookAsync(updateBookCommand);

            var updatedBook = await _bookService.GetBookAsync(1);

            Assert.NotNull(updatedBook);
            Assert.Equal(1, updatedBook.BookId);
            Assert.Equal("Test Title Updated", updatedBook.Title);
            Assert.Equal("Test Description Updated", updatedBook.Description);
        }
    }

    [Fact]
    public async Task UpdateBookAsync_IfRequestIsInvalid_ShouldThrowValidationException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();
            UpdateBookCommand updateBookCommand = new UpdateBookCommand();

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _bookService.UpdateBookAsync(updateBookCommand);
            });
        }
    }

    [Fact]
    public async Task DeleteBookAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();

            DeleteBookDto deletedBook = await _bookService.DeleteBookAsync(3);

            Assert.NotNull(deletedBook);
            Assert.Equal(true, deletedBook.Success);
            Assert.Equal("Successfully removed the book with 3 bookId.", deletedBook.Message);
        }
    }

    [Fact]
    public async Task DeleteBookAsync_IfBookDoesnotExist_ShouldThrowNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBookService _bookService = _fixture.Container.GetInstance<IBookService>();

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _bookService.DeleteBookAsync(111);
            });
        }
    }
}
