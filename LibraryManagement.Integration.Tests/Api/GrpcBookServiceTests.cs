using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.Interceptors;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Api.Services;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagement.Integration.Tests.Api;

public class GrpcBookServiceTests
{
    private readonly Mock<IBookService> _bookServiceMock;
    private readonly IMapper _mapper;
    private readonly GrpcBookService _grpcBookService;
    private readonly ExceptionHandlingInterceptor _interceptor;

    public GrpcBookServiceTests()
    {
        _bookServiceMock = new Mock<IBookService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GrpcBookMappingProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        config.AssertConfigurationIsValid();

        _grpcBookService = new GrpcBookService(_bookServiceMock.Object, _mapper);
        _interceptor = new ExceptionHandlingInterceptor();
    }

    [Fact]
    public async Task GetBook_IfBookExists_ShouldReturnEntity()
    {
        BookDto book = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Max Payne",
            CategoryId = 1,
            CategoryName = "Action",
            PublishedDate = "2000-12-31",
            PageCount = 200,
            IsAvailable = true
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.GetBookAsync(1, context.CancellationToken))
            .ReturnsAsync(book);

        BookGetRequest bookGetRequest = new BookGetRequest
        {
            BookId = 1
        };

        BookGetResponse result = await _grpcBookService.GetBook(bookGetRequest, context);
        Assert.Equal(book.AuthorName, result.Book.AuthorName);

        _bookServiceMock.Verify(s =>
            s.GetBookAsync(1, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetBook_IfBookDoesntExist_ShouldThrowException()
    {
        BookDto book = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Max Payne",
            CategoryId = 1,
            CategoryName = "Action",
            PublishedDate = "2000-12-31",
            PageCount = 200,
            IsAvailable = true
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.GetBookAsync(1, context.CancellationToken))
            .ReturnsAsync(book);

        BookGetRequest request = new BookGetRequest
        {
            BookId = 11
        };

        Assert.ThrowsAsync<NotFoundException>(async () => await _grpcBookService.GetBook(request, context));

        _bookServiceMock.Verify(s =>
            s.GetBookAsync(11, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateBook_IfCreateBookRequestIsValid_ShouldReturnCreatedEntity()
    {
        CreateBookRequest createBookRequest = new CreateBookRequest
        {
            Title = "Test Title",
            Isbn = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = "2000-12-31",
            PageCount = 200,
        };

        BookDto createdBook = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Max Payne",
            CategoryId = 1,
            CategoryName = "Action",
            PublishedDate = "2000-12-31",
            PageCount = 200,
            IsAvailable = true
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.CreateBookAsync(It.IsAny<CreateBookCommand>(), context.CancellationToken))
            .ReturnsAsync(createdBook);

        BookResponse createdGrpcBook = await _grpcBookService.CreateBook(createBookRequest, context);
        Assert.Equal(createdBook.AuthorName, createdGrpcBook.AuthorName);

        _bookServiceMock.Verify(s =>
            s.CreateBookAsync(It.IsAny<CreateBookCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateBook_IfCreateBookRequestIsInvalid_ShouldThrowException()
    {
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s => s.CreateBookAsync(It.IsAny<CreateBookCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));
        CreateBookRequest request = new CreateBookRequest();
        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(request, context, _grpcBookService.CreateBook));

        Assert.Equal("Unknown error: Unknown issue", grpcException.Status.Detail);

        _bookServiceMock.Verify(s => s.CreateBookAsync(It.IsAny<CreateBookCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateBook_IfUpdateBookRequestIsValid_ShouldReturnUpdatedEntity()
    {
        UpdateBookRequest updateBookRequest = new UpdateBookRequest
        {
            BookId = 1,
            Title = "Test Title",
            Description = "Test Description",
            CategoryId = 1,
            PublishedDate = "2000-12-31",
            PageCount = 200,
        };

        BookDto updatedBook = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Max Payne",
            CategoryId = 1,
            CategoryName = "Action",
            PublishedDate = "2000-12-31",
            PageCount = 200,
            IsAvailable = true
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.UpdateBookAsync(It.IsAny<UpdateBookCommand>(), context.CancellationToken))
            .ReturnsAsync(updatedBook);

        BookResponse updatedGrpcBook = await _grpcBookService.UpdateBook(updateBookRequest, context);
        Assert.Equal(updatedBook.Title, updatedGrpcBook.Title);

        _bookServiceMock.Verify(s =>
            s.UpdateBookAsync(It.IsAny<UpdateBookCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateBook_IfUpdateBookRequestIsinvalid_ShouldThrowException()
    {
        UpdateBookRequest updateBookRequest = new UpdateBookRequest
        {
            BookId = 1,
            Title = "Test Title",
            Description = "Test Description",
            CategoryId = 1,
            PublishedDate = "2000-12-31",
            PageCount = 200,
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.UpdateBookAsync(It.IsAny<UpdateBookCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));

        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(updateBookRequest, context, _grpcBookService.UpdateBook));

        Assert.Equal("Unknown error: Unknown issue", grpcException.Status.Detail);
    }

    [Fact]
    public async Task DeleteBook_IfBookExists_ShouldReturnEntity()
    {
        BookDeleteRequest bookDeleteRequest = new BookDeleteRequest
        {
            BookId = 1
        };
        DeleteBookDto deletedBookDto = new DeleteBookDto
        {
            Success = true,
            Message = "Success message"
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s => s.DeleteBookAsync(1, context.CancellationToken))
            .ReturnsAsync(deletedBookDto);

        BookGetRequest bookGetRequest = new BookGetRequest
        {
            BookId = 1
        };

        DeleteResponse result = await _grpcBookService.DeleteBook(bookDeleteRequest, context);
        Assert.Equal(deletedBookDto.Message, result.Message);

        _bookServiceMock.Verify(s =>
            s.DeleteBookAsync(1, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeletetBook_IfBookDoesntExist_ShouldThrowException()
    {
        BookDeleteRequest bookDeleteRequest = new BookDeleteRequest();
        DeleteBookDto deletedBookDto = new DeleteBookDto
        {
            Success = true,
            Message = "Success message"
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.DeleteBookAsync(1, context.CancellationToken))
            .ReturnsAsync(deletedBookDto);

        BookGetRequest request = new BookGetRequest
        {
            BookId = 11
        };

        Assert.ThrowsAsync<NotFoundException>(async () => await _grpcBookService.GetBook(request, context));

        _bookServiceMock.Verify(s =>
            s.GetBookAsync(11, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetBooks_IfBooskExist_ShouldReturnEntities()
    {
        BookDto book = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Max Payne",
            CategoryId = 1,
            CategoryName = "Action",
            PublishedDate = "2000-12-31",
            PageCount = 200,
            IsAvailable = true
        };

        PagedResult<BookDto> books =
            PagedResult<BookDto>.Create([book], 1, 1, 15);

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _bookServiceMock.Setup(s =>
            s.GetBooksAsync(It.IsAny<BookSearchArgs>(), context.CancellationToken))
            .ReturnsAsync(books);

        BookSearchRequest bookSearchRequest = new BookSearchRequest
        {
            SearchTerm = "Test",
            AuthorId = 1,
            CategoryId = 1,
            IsAvailable = true,
            PageNumber = 1,
            PageSize = 15
        };

        BookListResponse result = await _grpcBookService.GetBooks(bookSearchRequest, context);
        Assert.Equal(books.TotalCount, result.TotalCount);

        _bookServiceMock.Verify(s =>
            s.GetBooksAsync(It.IsAny<BookSearchArgs>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetBooks_IfBookGetRequestIsInvalid_ShouldThrowException()
    {
        BookDto book = new BookDto
        {
            BookId = 1,
            Title = "Test Title",
            ISBN = "1112223334445",
            Description = "Test Description",
            AuthorId = 1,
            AuthorName = "Max Payne",
            CategoryId = 1,
            CategoryName = "Action",
            PublishedDate = "2000-12-31",
            PageCount = 200,
            IsAvailable = true
        };
        PagedResult<BookDto> books =
            PagedResult<BookDto>.Create([book], 1, 1, 15);

        ServerCallContext context = Mock.Of<ServerCallContext>();

        BookSearchRequest bookSearchRequest = new BookSearchRequest();

        _bookServiceMock.Setup(s =>
            s.GetBooksAsync(It.IsAny<BookSearchArgs>(), context.CancellationToken))
            .ReturnsAsync(books);

        Assert.ThrowsAsync<NotFoundException>(async () => await _grpcBookService.GetBooks(bookSearchRequest, context));

        _bookServiceMock.Verify(s =>
            s.GetBooksAsync(It.IsAny<BookSearchArgs>(), context.CancellationToken),
            Times.Once);
    }
}
