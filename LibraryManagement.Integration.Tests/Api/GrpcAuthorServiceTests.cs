using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.Interceptors;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Api.Services;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Contract.QueryModels.Authors;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagement.Integration.Tests.Api;

public class GrpcAuthorServiceTests
{
    private readonly Mock<IAuthorService> _authorServiceMock;
    private readonly IMapper _mapper;
    private readonly GrpcAuthorService _grpcAuthorService;
    private readonly ExceptionHandlingInterceptor _interceptor;

    public GrpcAuthorServiceTests()
    {
        _authorServiceMock = new Mock<IAuthorService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GrpcAuthorMappingProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        config.AssertConfigurationIsValid();

        _grpcAuthorService = new GrpcAuthorService(_authorServiceMock.Object, _mapper);
        _interceptor = new ExceptionHandlingInterceptor();
    }

    [Fact]
    public async Task GetAuthor_IfAuthorExists_ShouldReturnEntity()
    {
        AuthorDto author = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31",
            IsActive = true,
            BookCount = 1
};
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.GetAuthorAsync(1, context.CancellationToken))
            .ReturnsAsync(author);

        AuthorGetRequest request = new AuthorGetRequest
        {
            AuthorId = 1
        };

        AuthorGetResponse result = await _grpcAuthorService.GetAuthor(request, context);
        Assert.Equal(author.FirstName, result.Author.FirstName);

        _authorServiceMock.Verify(s =>
            s.GetAuthorAsync(1, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetAuthor_IfAuthorDoesntExist_ShouldThrowException()
    {
        AuthorDto author = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31",
            IsActive = true,
            BookCount = 1
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.GetAuthorAsync(1, context.CancellationToken))
            .ReturnsAsync(author);

        AuthorGetRequest request = new AuthorGetRequest
        {
            AuthorId = 11
        };

        await Assert.ThrowsAsync<RpcException>
            (async () => await _interceptor.UnaryServerHandler(request, context, _grpcAuthorService.GetAuthor));

        _authorServiceMock.Verify(s =>
            s.GetAuthorAsync(11, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateAuthor_IfCreateAuthorRequestIsValid_ShouldReturnCreatedEntity()
    {
        CreateAuthorRequest createAuthorRequest = new CreateAuthorRequest
        {
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31"
        };

        AuthorDto createdAuthor = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31",
            IsActive = true,
            BookCount = 0
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken))
            .ReturnsAsync(createdAuthor);

        AuthorResponse createdGrpcAuthor = await _grpcAuthorService.CreateAuthor(createAuthorRequest, context);
        Assert.Equal(createdAuthor.FirstName, createdGrpcAuthor.FirstName);

        _authorServiceMock.Verify(s =>
            s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateAuthor_IfCreateAuthorRequestIsInvalid_ShouldThrowException()
    {
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s => s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));
        CreateAuthorRequest request = new CreateAuthorRequest();
        RpcException exception = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(request, context, _grpcAuthorService.CreateAuthor));

        Assert.Equal("Unknown error: Unknown issue", exception.Status.Detail);

        _authorServiceMock.Verify(s => s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAuthor_IfUpdateAuthorRequestIsValid_ShouldReturnUpdatedEntity()
    {
        UpdateAuthorRequest updateAuthorRequest = new UpdateAuthorRequest
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31",
            IsActive = true
        };

        AuthorDto updatedAuthor = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31",
            IsActive = true,
            BookCount = 0
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.UpdateAuthorAsync(It.IsAny<UpdateAuthorCommand>(), context.CancellationToken))
            .ReturnsAsync(updatedAuthor);

        AuthorResponse updatedGrpcAuthor = await _grpcAuthorService.UpdateAuthor(updateAuthorRequest, context);
        Assert.Equal(updatedAuthor.Biography, updatedGrpcAuthor.Biography);

        _authorServiceMock.Verify(s =>
            s.UpdateAuthorAsync(It.IsAny<UpdateAuthorCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAuthor_IfUpdateAuthorRequestIsinvalid_ShouldThrowException()
    {
        UpdateAuthorRequest updateAuthorRequest = new UpdateAuthorRequest
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "12-31-2000",
            IsActive = true
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.UpdateAuthorAsync(It.IsAny<UpdateAuthorCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));

        RpcException exception = await Assert.ThrowsAsync<RpcException>(() =>
            _interceptor.UnaryServerHandler(updateAuthorRequest, context, _grpcAuthorService.UpdateAuthor));

        Assert.Equal("Unknown error: Unknown issue", exception.Status.Detail);
    }

    [Fact]
    public async Task GetAuthors_IfAuthorsExist_ShouldReturnEntity()
    {
        AuthorDto author = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "2000-12-31",
            IsActive = true,
            BookCount = 1
        };
        PagedResult<AuthorDto> authors = PagedResult<AuthorDto>.Create([author], 1, 1, 15);

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.GetAuthorsAsync(It.IsAny<AuthorSearchArgs>(), context.CancellationToken))
            .ReturnsAsync(authors);

        AuthorSearchRequest authorSearchRequest = new AuthorSearchRequest
        {
            SearchTerm = "Max",
            IsActive = true,
            PageNumber = 1,
            PageSize = 15
        };

        AuthorListResponse result = await _grpcAuthorService.GetAuthors(authorSearchRequest, context);
        Assert.Equal(authors.TotalCount, result.TotalCount);

        _authorServiceMock.Verify(s =>
            s.GetAuthorsAsync(It.IsAny<AuthorSearchArgs>(), context.CancellationToken),
            Times.Once);
    }

    public async Task DeleteAuthor_IfAuthorExists_ShouldReturnSuccess()
    {
        AuthorDeleteRequest authorDeleteRequest = new AuthorDeleteRequest
        {
            AuthorId = 1
        };
        DeleteAuthorDto deletedAuthorDto = new DeleteAuthorDto
        {
            Success = true,
            Message = "Success message"
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s => s.DeleteAuthorAsync(1, context.CancellationToken))
            .ReturnsAsync(deletedAuthorDto);

        AuthorGetRequest authorGetRequest = new AuthorGetRequest
        {
            AuthorId = 1
        };

        DeleteResponse result = await _grpcAuthorService.DeleteAuthor(authorDeleteRequest, context);
        Assert.Equal(deletedAuthorDto.Message, result.Message);

        _authorServiceMock.Verify(s =>
            s.DeleteAuthorAsync(1, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAuthor_IfAuthorDoesntExist_ShouldThrowException()
    {
        AuthorDeleteRequest authorDeleteRequest = new AuthorDeleteRequest()
        {
            AuthorId = 11
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.DeleteAuthorAsync(It.IsAny<long>(), context.CancellationToken))
            .ThrowsAsync(new NotFoundException($"Unable to delete, no author found with 11 authorId."));

        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
           _interceptor.UnaryServerHandler(authorDeleteRequest, context, _grpcAuthorService.DeleteAuthor));

        Assert.Equal("Unable to delete, no author found with 11 authorId.", grpcException.Status.Detail);

        _authorServiceMock.Verify(s =>
            s.DeleteAuthorAsync(11, context.CancellationToken),
            Times.Once);
    }
}   
