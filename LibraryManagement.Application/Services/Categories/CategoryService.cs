using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Contract.QueryModels.Categories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private ICategoryRepository _categoryRepository;
    private IMapper _mapper;
    private IValidator<CreateCategoryCommand> _createCategoryCommandValidator;
    private IValidator<CategorySearchArgs> _categorySearchArgsValidator;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IMapper mapper,
        IValidator <CreateCategoryCommand> createCategoryCommandValidator,
        IValidator<CategorySearchArgs> categorySearchArgsValidator
        )
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _createCategoryCommandValidator = createCategoryCommandValidator;
        _categorySearchArgsValidator = categorySearchArgsValidator;
    }

    public async Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken)
                   ?? throw new NotFoundException($"Can't find the category with {categoryId} categoryId!");

        return _mapper.Map<Category, CategoryDto>(category);
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(CategorySearchArgs categorySearchArgs, CancellationToken cancellationToken = default)
    {
        await _categorySearchArgsValidator.ValidateAndThrowAsync(categorySearchArgs, cancellationToken);
        var categories = await _categoryRepository.FindAsync(categorySearchArgs, cancellationToken);
        
        List<CategoryDto> mappedCategories = new List<CategoryDto>();
        foreach (var category in categories)
        {
            CategoryDto mappedCategory = _mapper.Map<Category, CategoryDto>(category);
            mappedCategories.Add(mappedCategory);
        }
        return mappedCategories;
    }

    public async Task<List<CategoryDto>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetCategoryTreeAsync(includeInactive, cancellationToken);
        var mappedCategories = categories.Select(_mapper.Map<CategoryDto>).ToList();

        return mappedCategories;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken = default)
    {
        await _createCategoryCommandValidator.ValidateAndThrowAsync(createCategoryCommand, cancellationToken);
        Category? parent = null;
        if (createCategoryCommand.ParentCategoryId.HasValue)
        {
            parent = await _categoryRepository.GetByIdAsync(createCategoryCommand.ParentCategoryId.Value, cancellationToken)
                         ?? throw new NotFoundException($"Can't find the {createCategoryCommand.ParentCategoryId.Value} category!");
        }
        var category = new Category(
            createCategoryCommand.Name,
            createCategoryCommand.Description,
            parent?.CategoryId
            );
        await _categoryRepository.AddAsync(category, cancellationToken);
        category = await _categoryRepository.GetByIdAsync(category.CategoryId, cancellationToken);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<string> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        Category category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken) 
            ?? throw new NotFoundException($"Can't find the {categoryId} category!");
        return $"category: {category.Name}, activityStatus: {category.IsActive}," +
            $"hasParent: {category.ParentCategory.Name}, hasSubcategories: {category.SubCategories.Count}.";
    }
}
