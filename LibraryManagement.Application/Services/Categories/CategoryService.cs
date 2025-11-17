using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Category;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Categories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;
        private IValidator<CreateCategoryCommand> _createCategoryCommandValidator;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IValidator <CreateCategoryCommand> createCategoryCommandValidator
            )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createCategoryCommandValidator = createCategoryCommandValidator;
        }

        public async Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken)
                       ?? throw new NotFoundException($"Can't find the {categoryId} category!");

            return _mapper.Map<Category, CategoryDto>(category);
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(CategorySearchArgs categorySearchArgs, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            List<CategoryDto> mappedCategories = new List<CategoryDto>();
            foreach (var category in categories)
            {
                CategoryDto mappedCategory = _mapper.Map<Category, CategoryDto>(category);
                mappedCategories.Append(mappedCategory);
            }
            return mappedCategories;
        }

        public async Task<List<CategoryDto>> GetCategoryTreeAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken)
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
                parent?.CategoryId,
                createCategoryCommand.SortOrder,
                true
                );
            await _categoryRepository.AddAsync(category, cancellationToken);
            return _mapper.Map<Category, CategoryDto>(category);
        }

        public async Task<string> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken) 
                ?? throw new NotFoundException($"Can't find the {categoryId} category!");
            return $"category: {category.Name}, activityStatus: {category.IsActive}," +
                $"hasParent: {category.ParentCategory.Name}, hasSubcategories: {category.SubCategories.Count}.";
        }
    }
}
