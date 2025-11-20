using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Api.Services;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
public class GrpcAuthorServiceTests
{
    private readonly Mock<IAuthorService> _authorServiceMock;
    private readonly IMapper _mapper;
    private readonly GrpcAuthorService _grpcAuthorService;

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
    }

    [Fact]
    public async Task GetAuthor_IfAuthorExists()
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
        var context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.GetAuthorAsync(1, context.CancellationToken))
            .ReturnsAsync(author);

        AuthorGetRequest request = new AuthorGetRequest
        {
            AuthorId = 1
        };

        var result = await _grpcAuthorService.GetAuthor(request, context);
        Assert.Equal(author.FirstName, result.Author.FirstName);

        _authorServiceMock.Verify(s =>
            s.GetAuthorAsync(1, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetAuthor_IfAuthorDoesntExist()
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
        var context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.GetAuthorAsync(1, context.CancellationToken))
            .ReturnsAsync(author);

        AuthorGetRequest request = new AuthorGetRequest
        {
            AuthorId = 11
        };  

        Assert.ThrowsAsync<NotFoundException>(async () => await _grpcAuthorService.GetAuthor(request, context));
        
        _authorServiceMock.Verify(s =>
            s.GetAuthorAsync(11, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateAuthor_IfAuthorIsValid_ShouldReturnCreatedEntity()
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
        var context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s =>
            s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken))
            .ReturnsAsync(createdAuthor);

        var createdGrpcAuthor = await _grpcAuthorService.CreateAuthor(createAuthorRequest, context);
        Assert.Equal(createdAuthor.FirstName, createdGrpcAuthor.FirstName);

        _authorServiceMock.Verify(s =>
            s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateAuthor_IfAuthorIsInvalid_ShouldThrowException()
    {
        CreateAuthorRequest createAuthorRequest = new CreateAuthorRequest
        {
            FirstName = "Max",
            LastName = "Payne",
            Biography = "Max Payne Bio",
            DateOfBirth = "12-31-2000"
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
        var context = Mock.Of<ServerCallContext>();

        _authorServiceMock.Setup(s => s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken))
            .ReturnsAsync(createdAuthor);

        Assert.ThrowsAsync<RpcException>(async () => await _grpcAuthorService.CreateAuthor(createAuthorRequest, context));

        _authorServiceMock.Verify(s => s.CreateAuthorAsync(It.IsAny<CreateAuthorCommand>(), context.CancellationToken),
            Times.Once);
    }
}   
