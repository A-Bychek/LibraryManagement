using FluentAssertions;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Integration.Tests.Base;
using LibraryManagement.Shared;

namespace LibraryManagement.Integration.Tests.Api.Mappings;

public class GrpcBookMappingTests : AutoMapperTestBase
{
    [Fact]
    public void BookDto_To_BookResponse_ShouldMapCorrectly()
    {
        BookDto bookDto = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1111111111111",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Test First Name Test Last Name",
            CategoryId = 1,
            CategoryName = "Test Category",
            PublishedDate = "2020-01-01",
            PageCount = 100,
            IsAvailable = true
        };

        var bookResponse = _mapper.Map<BookResponse>(bookDto);

        bookResponse.Should().NotBeNull();
        bookResponse.BookId.Should().Be(bookDto.BookId);
        bookResponse.Title.Should().Be(bookDto.Title);
        bookResponse.Isbn.Should().Be(bookDto.ISBN);
        bookResponse.Description.Should().Be(bookDto.Description);
        bookResponse.AuthorId.Should().Be(bookDto.AuthorId);
        bookResponse.AuthorName.Should().Be(bookDto.AuthorName);
        bookResponse.CategoryId.Should().Be(bookDto.CategoryId);
        bookResponse.CategoryName.Should().Be(bookDto.CategoryName);
        bookResponse.PublishedDate.Should().Be(bookDto.PublishedDate);
        bookResponse.PageCount.Should().Be(bookDto.PageCount);
        bookResponse.IsAvailable.Should().Be(bookDto.IsAvailable);
    }

    [Fact]
    public void CreateBookRequest_To_CreateBookCommand_ShouldMapCorrectly()
    {
        CreateBookRequest createBookRequest = new CreateBookRequest
        {
            Title = "Test Title",
            Isbn = "1111111111111",
            Description = "Test Description",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = "2000-01-01",
            PageCount = 100
        };

        var createBookCommand = _mapper.Map<CreateBookCommand>(createBookRequest);

        createBookCommand.Should().NotBeNull();
        createBookCommand.Title.Should().Be(createBookRequest.Title);
        createBookCommand.ISBN.Should().Be(createBookRequest.Isbn);
        createBookCommand.Description.Should().Be(createBookRequest.Description);
        createBookCommand.AuthorId.Should().Be(createBookRequest.AuthorId);
        createBookCommand.CategoryId.Should().Be(createBookRequest.CategoryId);
        createBookCommand.PublishedDate.Should().Be(createBookRequest.PublishedDate);
        createBookCommand.PageCount.Should().Be(createBookRequest.PageCount);
    }

    [Fact]
    public void UpdateBookRequest_To_UpdateBookCommand_ShouldMapCorrectly()
    {
        UpdateBookRequest updateBookRequest = new UpdateBookRequest
        {
            BookId = 1,
            Title = "Test Title",
            Description = "Test Description",
            CategoryId = 1,
            PublishedDate = "2020-01-01",
            PageCount = 100
        };

        var updateBookCommand = _mapper.Map<UpdateBookCommand>(updateBookRequest);

        updateBookCommand.Should().NotBeNull();
        updateBookCommand.BookId.Should().Be(updateBookRequest.BookId);
        updateBookCommand.Title.Should().Be(updateBookRequest.Title);
        updateBookCommand.Description.Should().Be(updateBookRequest.Description);
        updateBookCommand.CategoryId.Should().Be(updateBookRequest.CategoryId);
        updateBookCommand.PublishedDate.Should().Be(updateBookRequest.PublishedDate);
        updateBookCommand.PageCount.Should().Be(updateBookRequest.PageCount);
    }

    [Fact]
    public void BookSearchRequest_To_BookSearchArgs_ShouldMapCorrectly()
    {
        BookSearchRequest bookSearchRequest = new BookSearchRequest
        {
            SearchTerm = "TestTerm",
            AuthorId = 1,
            CategoryId = 1,
            IsAvailable = true,
            PageSize = 15
        };

        var bookSearchArgs = _mapper.Map<BookSearchArgs>(bookSearchRequest);

        bookSearchArgs.Should().NotBeNull();
        bookSearchArgs.SearchTerm.Should().Be(bookSearchRequest.SearchTerm);
        bookSearchArgs.AuthorId.Should().Be(bookSearchRequest.AuthorId);
        bookSearchArgs.CategoryId.Should().Be(bookSearchRequest.CategoryId);
        bookSearchArgs.IsAvailable.Should().Be(bookSearchRequest.IsAvailable);
        bookSearchArgs.PageNumber.Equals(1);
        bookSearchArgs.PageSize.Should().Be(bookSearchRequest.PageSize);
    }

    [Fact]
    public void DeleteBookDto_To_DeleteResponse_ShouldMapCorrectly()
    {
        DeleteBookDto deleteBookDto = new DeleteBookDto
        {
            Success = true,
            Message = "Test Message"
        };

        var deleteResponse = _mapper.Map<DeleteResponse>(deleteBookDto);

        deleteResponse.Should().NotBeNull();
        deleteResponse.Success.Should().Be(deleteBookDto.Success);
        deleteResponse.Message.Should().Be(deleteBookDto.Message);
    }

    [Fact]
    public void PagedResultBookDto_To_BookListResponse_ShouldMapCorrectly()
    {
        BookDto bookDto = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1111111111111",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Test First Name Test Last Name",
            CategoryId = 1,
            CategoryName = "Test Category",
            PublishedDate = "2020-01-01",
            PageCount = 100,
            IsAvailable = true
        };

        PagedResult<BookDto> pagedResultBookDto = PagedResult<BookDto>.Create([bookDto], 1, 1, 15);

        var mappedBooks = pagedResultBookDto.Items.Select(_mapper.Map<BookResponse>);

        var bookListResponse = _mapper.Map<BookListResponse>(pagedResultBookDto,
            opt => opt.AfterMap((src, dest) => dest.Books.AddRange(mappedBooks)));

        bookListResponse.Should().NotBeNull();
        bookListResponse.Books.Should().HaveCount(1);
        bookListResponse.Books[0].BookId.Should().Be(bookDto.BookId);
        bookListResponse.Books[0].Title.Should().Be(bookDto.Title);
        bookListResponse.Books[0].Isbn.Should().Be(bookDto.ISBN);
        bookListResponse.Books[0].Description.Should().Be(bookDto.Description);
        bookListResponse.Books[0].AuthorId.Should().Be(bookDto.AuthorId);
        bookListResponse.Books[0].AuthorName.Should().Be(bookDto.AuthorName);
        bookListResponse.Books[0].CategoryId.Should().Be(bookDto.CategoryId);
        bookListResponse.Books[0].CategoryName.Should().Be(bookDto.CategoryName);
        bookListResponse.Books[0].PublishedDate.Should().Be(bookDto.PublishedDate);
        bookListResponse.Books[0].PageCount.Should().Be(bookDto.PageCount);
        bookListResponse.Books[0].IsAvailable.Should().Be(bookDto.IsAvailable);
        bookListResponse.TotalCount.Should().Be(pagedResultBookDto.TotalCount);
        bookListResponse.PageNumber.Should().Be(pagedResultBookDto.PageNumber);
        bookListResponse.PageSize.Should().Be(pagedResultBookDto.PageSize);
    }
}
