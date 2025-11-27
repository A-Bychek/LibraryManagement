using FluentValidation;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.QueryModels.Categories;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Application;

public class CategoryServiceTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public CategoryServiceTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCategoryAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryService _categoryService = _fixture.Container.GetInstance<ICategoryService>();

            CategoryDto category = await _categoryService.GetCategoryAsync(1);

            Assert.NotNull(category);
            Assert.Equal(1, category.CategoryId);
            Assert.Equal("Test Category 1", category.Name);
            Assert.Equal("Test Description 1", category.Description);
            Assert.True(category.IsActive);
        }
    }
    
    [Fact]
    public async Task GetCategoryAsync_WhenIdDoesnotExist_ShouldThrowsNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryService _categoryService = _fixture.Container.GetInstance<ICategoryService>();

            await Assert.ThrowsAsync<NotFoundException>(async () => await _categoryService.GetCategoryAsync(111));
        }
    }
    
    [Fact]
    public async Task GetCategoriesAsync_IfRequestIsValid_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryService _categoryService = _fixture.Container.GetInstance<ICategoryService>();
            CategorySearchArgs args = new CategorySearchArgs
            {
                SearchTerm = "Category 2",
                IsActive = true,
            };

            List<CategoryDto>? categories = await _categoryService.GetCategoriesAsync(args);

            Assert.NotNull(categories);
            Assert.Single(categories);
        }
    }
    
    [Fact]
    public async Task CreateCategoryAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryService _categoryService = _fixture.Container.GetInstance<ICategoryService>();
            CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand
            {
                Name = "Test Category 4",
                Description = "Test Description 4",
                SortOrder = 0
            };

            CategoryDto addedCategory = await _categoryService.CreateCategoryAsync(createCategoryCommand);

            Assert.NotNull(addedCategory);
            Assert.Equal(4, addedCategory.CategoryId);
            Assert.Equal("Test Category 4", addedCategory.Name);
            Assert.Equal("Test Description 4", addedCategory.Description);
        }
    }

    [Fact]
    public async Task CreateCategoryAsync_IfRequestIsInvalid_ShouldThrowValidationException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryService _categoryService = _fixture.Container.GetInstance<ICategoryService>();
            CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand();

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _categoryService.CreateCategoryAsync(createCategoryCommand);
            });
        }
    }

    [Fact]
    public async Task GetCategoryTreeAsync_IfRequestIsValid_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryService _categoryService = _fixture.Container.GetInstance<ICategoryService>();

            List<CategoryDto>? categories = await _categoryService.GetCategoryTreeAsync(true);

            Assert.NotNull(categories);
            Assert.Equal(4, categories.Count);
        }
    }
}
