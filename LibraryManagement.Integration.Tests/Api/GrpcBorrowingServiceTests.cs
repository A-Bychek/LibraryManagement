using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Api.Services;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.QueryModels.Borrowings;
using LibraryManagement.Contract.Borrowings;
using LibraryManagement.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using LibraryManagement.Api.Interceptors;

namespace LibraryManagement.Integration.Tests.Api;

public class GrpcBorrowingServiceTests
{
    private readonly Mock<IBorrowingService> _borrowingServiceMock;
    private readonly IMapper _mapper;
    private readonly GrpcBorrowingService _grpcBorrowingService;
    private readonly ExceptionHandlingInterceptor _interceptor;

    public GrpcBorrowingServiceTests()
    {
        _borrowingServiceMock = new Mock<IBorrowingService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GrpcBorrowingMappingProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        config.AssertConfigurationIsValid();

        _grpcBorrowingService = new GrpcBorrowingService(_borrowingServiceMock.Object, _mapper);
        _interceptor = new ExceptionHandlingInterceptor();
    }

    [Fact]
    public async Task BorrowBook_IfBookAvailable_ShouldReturnEntity()
    {
        BorrowingDto borrowing = new BorrowingDto
        {
            BorrowingId = 1,
            BookId = 1,
            BookTitle = "Test Title 1",
            UserId = 1,
            BorrowDate = "2025-01-01",
            DueDate = "2025-01-11",
            ReturnDate = "",
            Status = "Active",
            FineAmount = 0
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.BorrowBookAsync(It.IsAny<BorrowBookCommand>(), context.CancellationToken))
            .ReturnsAsync(borrowing);

        BorrowBookRequest borrowBookRequest = new BorrowBookRequest
        {
            BookId = 1,
            UserId = 1,
            DaysToReturn = 10
        };

        BorrowingResponse result = await _grpcBorrowingService.BorrowBook(borrowBookRequest, context);
        Assert.Equal(borrowing.DueDate, result.DueDate);

        _borrowingServiceMock.Verify(s =>
            s.BorrowBookAsync(It.IsAny<BorrowBookCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task BorrowBook_IfBorrowBookRequestIsInvalid_ShouldThrowException()
    {
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s => s.BorrowBookAsync(It.IsAny<BorrowBookCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));
        BorrowBookRequest request = new BorrowBookRequest();
        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(request, context, _grpcBorrowingService.BorrowBook));

        Assert.Equal("Unknown error: Unknown issue", grpcException.Status.Detail);

        _borrowingServiceMock.Verify(s => s.BorrowBookAsync(It.IsAny<BorrowBookCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task ReturnBook_IfReturnBookRequestIsValid_ShouldReturnEntity()
    {
        ReturnBookRequest returnBookRequest = new ReturnBookRequest
        {
            BorrowingId = 1
        };

        BorrowingDto returnedBorrowing = new BorrowingDto
        {
            BorrowingId = 1,
            BookId = 1,
            BookTitle = "Test Title 1",
            UserId = 1,
            BorrowDate = "2025-01-01",
            DueDate = "2025-01-11",
            ReturnDate = "",
            Status = "Returned",
            FineAmount = 0
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.ReturnBookAsync(It.IsAny<ReturnBookCommand>(), context.CancellationToken))
            .ReturnsAsync(returnedBorrowing);

        BorrowingResponse returnedGrpcBook = await _grpcBorrowingService.ReturnBook(returnBookRequest, context);
        Assert.Equal(returnedBorrowing.BookTitle, returnedGrpcBook.BookTitle);

        _borrowingServiceMock.Verify(s =>
            s.ReturnBookAsync(It.IsAny<ReturnBookCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task ReturnBook_IfReturnBookRequestIsinvalid_ShouldThrowException()
    {
        ReturnBookRequest returnBookRequest = new ReturnBookRequest
        {
            BorrowingId = 1
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.ReturnBookAsync(It.IsAny<ReturnBookCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));

        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(returnBookRequest, context, _grpcBorrowingService.ReturnBook));

        Assert.Equal("Unknown error: Unknown issue", grpcException.Status.Detail);
    }

    [Fact]
    public async Task GetUserBorrowings_IfRequestIsValid_ShouldReturnListOfEntities()
    {
        UserBorrowingsRequest userBorrowingsRequest = new UserBorrowingsRequest
        {
            UserId = 1,
            Status = "Active"
        };

        var dto = new BorrowingDto
        {
            BorrowingId = 1,
            BookId = 1,
            BookTitle = "Test Title 1",
            UserId = 1,
            BorrowDate = "2025-01-01",
            DueDate = "2025-01-11",
            ReturnDate = "",
            Status = "Returned",
            FineAmount = 0
        };

        PagedResult<BorrowingDto> userBorrowings =
            PagedResult<BorrowingDto>.Create([ dto ], 1, 1, 15);

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.GetUserBorrowingsAsync(It.IsAny<BorrowingSearchArgs>(), context.CancellationToken))
            .ReturnsAsync(userBorrowings);

        BorrowingListResponse grpcUserBorrowings = await _grpcBorrowingService.GetUserBorrowings(userBorrowingsRequest, context);
        Assert.Equal(userBorrowings.PageSize, grpcUserBorrowings.PageSize);

        _borrowingServiceMock.Verify(s =>
            s.GetUserBorrowingsAsync(It.IsAny<BorrowingSearchArgs>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetUserBorrowings_IfUserBorrowingsRequestIsinvalid_ShouldThrowException()
    {
        UserBorrowingsRequest userBorrowingsRequest = new UserBorrowingsRequest
        {
            UserId = 1,
            Status = "Active"
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.GetUserBorrowingsAsync(It.IsAny<BorrowingSearchArgs>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));

        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(userBorrowingsRequest, context, _grpcBorrowingService.GetUserBorrowings));

        Assert.Equal("Unknown error: Unknown issue", grpcException.Status.Detail);
    }

    [Fact]
    public async Task GetOverdueBooks_IfRequestIsValid_ShouldReturnListOfEntities()
    {
        OverdueBooksRequest overdueBooksRequest = new OverdueBooksRequest
        {
            PageNumber = 1,
            PageSize = 15
        };
        
        var dto = new BorrowingDto
        {
            BorrowingId = 1,
            BookId = 1,
            BookTitle = "Test Title 1",
            UserId = 1,
            BorrowDate = "2025-01-01",
            DueDate = "2025-01-11",
            ReturnDate = "",
            Status = "Overdue",
            FineAmount = 0
        };

        List<BorrowingDto> userBorrowings = [dto];
        
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.GetOverdueBooksAsync(context.CancellationToken))
            .ReturnsAsync(userBorrowings);

        BorrowingListResponse grpcUserBorrowings = await _grpcBorrowingService.GetOverdueBooks(overdueBooksRequest, context);
        Assert.Equal(userBorrowings.Count, grpcUserBorrowings.TotalCount);

        _borrowingServiceMock.Verify(s =>
            s.GetOverdueBooksAsync(context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetOverdueBooks_IfOverdueBooksRequestIsinvalid_ShouldThrowException()
    {
        OverdueBooksRequest overdueBooksRequest = new OverdueBooksRequest
        {
            PageNumber = -1,
            PageSize = 15
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _borrowingServiceMock.Setup(s =>
            s.GetOverdueBooksAsync(context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));

        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(overdueBooksRequest, context, _grpcBorrowingService.GetOverdueBooks));

        Assert.Equal("Unknown error: Unknown issue", grpcException.Status.Detail);
    }
}
