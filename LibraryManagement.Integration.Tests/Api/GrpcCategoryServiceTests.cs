using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Api.Services;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Categories;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Contract.QueryModels.Categories;
using LibraryManagement.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagement.Integration.Tests.Api;

public class GrpcCategoryServiceTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly IMapper _mapper;
    private readonly GrpcCategoryService _grpcCategoryService;

    public GrpcCategoryServiceTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GrpcCategoryMappingProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        config.AssertConfigurationIsValid();

        _grpcCategoryService = new GrpcCategoryService(_categoryServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task GetCategories_IfCategorySearchRequestIsValid_ShouldReturnEntities()
    {
        CategorySearchRequest getCategoriesRequest = new CategorySearchRequest
        {
            SearchTerm = "test",
            ParentCategoryId = 1,
            IsActive = true
        };

        CategoryDto category = new CategoryDto
        {
            CategoryId = 2,
            Name = "Test Category",
            Description = "Test Description",
            ParentCategoryId = 1,
            ParentCategoryName = "Test Parent Category",
            SortOrder = 0,
            IsActive = true,
            BookCount = 1
        };
        List<CategoryDto> categories = [category];
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _categoryServiceMock.Setup(s =>
            s.GetCategoriesAsync(It.IsAny<CategorySearchArgs>(), context.CancellationToken))
            .ReturnsAsync(categories);

        BookGetRequest bookGetRequest = new BookGetRequest
        {
            BookId = 1
        };

        CategoryListResponse result = await _grpcCategoryService.GetCategories(getCategoriesRequest, context);
        Assert.Equal(categories.Count, result.Categories.Count);

        _categoryServiceMock.Verify(s =>
            s.GetCategoriesAsync(It.IsAny<CategorySearchArgs>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCategories_IfGetCategoriesRequestIsInvalid_ShouldThrowException()
    {
        CategorySearchRequest getCategoriesRequest = new CategorySearchRequest
        {
            SearchTerm = "test",
            ParentCategoryId = 1,
            IsActive = true
        };

        CategoryDto category = new CategoryDto
        {
            CategoryId = 2,
            Name = "Test Category",
            Description = "Test Description",
            ParentCategoryId = 1,
            ParentCategoryName = "Test Parent Category",
            SortOrder = 0,
            IsActive = true,
            BookCount = 1
        };
        List<CategoryDto> categories = [category];
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _categoryServiceMock.Setup(s =>
            s.GetCategoriesAsync(It.IsAny<CategorySearchArgs>(), context.CancellationToken))
            .ReturnsAsync(categories);


        Assert.ThrowsAsync<NotFoundException>(async () => await _grpcCategoryService.GetCategories(getCategoriesRequest, context));

        _categoryServiceMock.Verify(s =>
            s.GetCategoriesAsync(It.IsAny<CategorySearchArgs>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateCategory_IfCreateCategoryRequestIsValid_ShouldReturnCreatedEntity()
    {
        CreateCategoryRequest createCategoryRequest = new CreateCategoryRequest
        {
            Name = "Test Name",
            Description = "Test Description",
            ParentCategoryId = 1,
            SortOrder = 0
        };

        CategoryDto createdCategory = new CategoryDto
        {
            CategoryId = 2,
            Name = "Test Category",
            Description = "Test Description",
            ParentCategoryId = 1,
            ParentCategoryName = "Test Parent Category",
            SortOrder = 0,
            IsActive = true,
            BookCount = 1
        };
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _categoryServiceMock.Setup(s =>
            s.CreateCategoryAsync(It.IsAny<CreateCategoryCommand>(), context.CancellationToken))
            .ReturnsAsync(createdCategory);

        CategoryResponse createdGrpcCategory = await _grpcCategoryService.CreateCategory(createCategoryRequest, context);
        Assert.Equal(createdCategory.Name, createdGrpcCategory.Name);

        _categoryServiceMock.Verify(s =>
            s.CreateCategoryAsync(It.IsAny<CreateCategoryCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateCategory_IfCreateCategoryRequestIsInvalid_ShouldThrowException()
    {
        ServerCallContext context = Mock.Of<ServerCallContext>();

        _categoryServiceMock.Setup(s => s.CreateCategoryAsync(It.IsAny<CreateCategoryCommand>(), context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));
        CreateCategoryRequest request = new CreateCategoryRequest();
        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _grpcCategoryService.CreateCategory(request, context));

        Assert.Equal(StatusCode.Unknown, grpcException.StatusCode);

        _categoryServiceMock.Verify(s => s.CreateCategoryAsync(It.IsAny<CreateCategoryCommand>(), context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCategoryTree_IfGetCategoryTreeRequestIsValid_ShouldReturnEntityTree()
    {
        CategoryDto categoryDto = new CategoryDto
        {
            CategoryId = 2,
            Name = "Test Category",
            Description = "Test Description",
            ParentCategoryId = 1,
            ParentCategoryName = "Test Parent Category",
            SortOrder = 0,
            IsActive = true,
            BookCount = 1
        };

        List<CategoryDto> categoryTree = [categoryDto];

        CategoryTreeRequest categoryTreeRequest = new CategoryTreeRequest
        {
            IncludeInactive = true
        };

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _categoryServiceMock.Setup(s =>
            s.GetCategoryTreeAsync(true, context.CancellationToken))
            .ReturnsAsync(categoryTree);

        CategoryTreeResponse grpcCategoryTree = await _grpcCategoryService.GetCategoryTree(categoryTreeRequest, context);
        Assert.Equal(categoryTree.Count, grpcCategoryTree.Categories.Count);

        _categoryServiceMock.Verify(s =>
            s.GetCategoryTreeAsync(true, context.CancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCategoryTree_IfGetCategoryTreeRequestIsinvalid_ShouldThrowException()
    {
        CategoryTreeRequest categoryTreeRequest = new CategoryTreeRequest();

        ServerCallContext context = Mock.Of<ServerCallContext>();

        _categoryServiceMock.Setup(s =>
            s.GetCategoryTreeAsync(true, context.CancellationToken))
            .ThrowsAsync(new Exception("Unknown issue"));

        RpcException grpcException = await Assert.ThrowsAsync<RpcException>(() =>
            _grpcCategoryService.GetCategoryTree(categoryTreeRequest, context));

        Assert.Equal(StatusCode.Unknown, grpcException.StatusCode);

    }
}
