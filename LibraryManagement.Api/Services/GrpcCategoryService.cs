using AutoMapper;
using Grpc.Core;
using FluentValidation;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Categories;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Contract.QueryModels.Categories;
using Serilog;

namespace LibraryManagement.Api.Services;

public class GrpcCategoryService : CategoryService.CategoryServiceBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public GrpcCategoryService(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _logger = Log.Logger;
    }

    public override async Task<CategoryListResponse> GetCategories(CategorySearchRequest request, ServerCallContext context)
    {
        var categorySearchArgs = _mapper.Map<CategorySearchArgs>(request);

        var categories = await _categoryService.GetCategoriesAsync(categorySearchArgs, context.CancellationToken);
        var mappedCategories = new CategoryListResponse();
        mappedCategories.Categories.AddRange(categories.Select(_mapper.Map<CategoryResponse>));
        return mappedCategories;
    }

    public override async Task<CategoryResponse> CreateCategory(CreateCategoryRequest createCategoryRequest, ServerCallContext context)
    {
        var createCategoryCommand = _mapper.Map<CreateCategoryCommand>(createCategoryRequest);

        var category = await _categoryService.CreateCategoryAsync(createCategoryCommand, context.CancellationToken);
            
        _logger.Information($"Category entity has been created: ID: {category.CategoryId}," +
            $"Name: {category.Name}, Description: {category.Description}, Parent category ID {category.ParentCategoryId}," +
            $"parent category name: {category.ParentCategoryName}, isActive: {category.IsActive}");
        return _mapper.Map<CategoryResponse>(category);
    }

    public override async Task<CategoryTreeResponse> GetCategoryTree(CategoryTreeRequest request, ServerCallContext context)
    {
        var categories = await _categoryService.GetCategoryTreeAsync(request.IncludeInactive, context.CancellationToken);

        CategoryTreeResponse mappedCategories = new CategoryTreeResponse();
        mappedCategories.Categories.AddRange(categories.Select(x => _mapper.Map<CategoryResponse>(x)));

        _logger.Information($"{categories.Count} categories have been found.");
        return mappedCategories;
    }
}
