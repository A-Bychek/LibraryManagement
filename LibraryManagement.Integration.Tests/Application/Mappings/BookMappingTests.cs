using FluentAssertions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Integration.Tests.Base;

namespace LibraryManagement.Integration.Tests.Application.Mappings;

public class BookMappingTests : AutoMapperTestBase
{
    [Fact]
    public void Book_To_BookDto_ShouldMapCorrectly()
    {
        Book book = new Book
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1111111111111",
            Description = "Test Description",
            AuthorId = 1,
            Author = new Author
            {
                AuthorId = 1,
                FirstName = "Test First Name",
                LastName = "Test Last Name",
                Biography = " Test Biography",
                DateOfBirth = new DateTime(1970, 01, 01),
                IsActive = true
            },
            CategoryId = 1,
            Category = new Category
            {
                CategoryId = 1,
                Name = "Test Category",
                Description = "Test Description",
            },
            PublishedDate = new DateTime(2000, 01, 01),
            PageCount = 100,
            IsAvailable = true,
            CreatedDate = new DateTime(2005, 01, 01),
            UpdatedDate = new DateTime(2010, 01, 01)
        };

        var bookDto = _mapper.Map<BookDto>(book);

        bookDto.Should().NotBeNull();
        bookDto.BookId.Should().Be(book.BookId);
        bookDto.Title.Should().Be(book.Title);
        bookDto.ISBN.Should().Be(book.ISBN);
        bookDto.Description.Should().Be(book.Description);
        bookDto.AuthorId.Should().Be(book.AuthorId);
        bookDto.AuthorName.Equals("Test First Name Test Last Name");
        bookDto.CategoryId.Should().Be(book.CategoryId);
        bookDto.CategoryName.Equals("Test Category");
        bookDto.PublishedDate.Should().Be(book.PublishedDate.ToString());
        bookDto.PageCount.Should().Be(book.PageCount);
        bookDto.IsAvailable.Should().Be(book.IsAvailable);
    }

    [Fact]
    public void CreateBookCommand_To_Book_ShouldMapCorrectly()
    {
        CreateBookCommand createBookCommand = new CreateBookCommand
        {
            Title = "Test Title",
            ISBN = "1111111111111",
            Description = "Description",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = "1980-01-01",
            PageCount = 100
        };

        var book = _mapper.Map<Book>(createBookCommand);

        book.Should().NotBeNull();
        book.BookId.Equals(1);
        book.Title.Should().Be(book.Title);
        book.ISBN.Should().Be(book.ISBN);
        book.Description.Should().Be(book.Description);
        book.AuthorId.Should().Be(book.AuthorId);
        book.CategoryId.Should().Be(book.CategoryId);
        book.PublishedDate.Should().Be(book.PublishedDate);
        book.PageCount.Should().Be(book.PageCount);
        book.IsAvailable.Equals(true);
        book.CreatedDate.Should().BeBefore(DateTime.UtcNow);
        book.UpdatedDate.Should().BeNull();
    }
}
