using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Contract.QueryModels.Categories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Integration.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Infrastructure;

public class CategoryRepositoryTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public CategoryRepositoryTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();

            Category? category = await _categoryRepository.GetByIdAsync(1);

            Assert.NotNull(category);
            Assert.Equal(1, category.CategoryId);
            Assert.Equal("Test Category 1", category.Name);
            Assert.Equal("Test Description 1", category.Description);
            Assert.Equal(true, category.IsActive);
        }
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdDoesnotExist_ShouldReturnNull()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();

            Category? category = await _categoryRepository.GetByIdAsync(111);
            Assert.Null(category);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();
            Category category = new Category
            {
                Name = "Test Category 4",
                Description = "Test Description 4",
                SortOrder = 0,
                IsActive = true
            };

            Category? addedCategory = await _categoryRepository.AddAsync(category);

            Assert.NotNull(addedCategory);
            Assert.Equal(4, addedCategory.CategoryId);
            Assert.Equal("Test Category 4", addedCategory.Name);
            Assert.Equal("Test Description 4", addedCategory.Description);
            Assert.Equal(true, addedCategory.IsActive);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Category category = new Category();

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _categoryRepository.AddAsync(category);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task GetCategoryTreeAsync_WhenIdsExist_ShouldReturnEnitities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();
            IEnumerable<Category> categories = await _categoryRepository.GetCategoryTreeAsync(true);

            Assert.NotNull(categories);
            Assert.Equal(3, categories.Count());
        }
    }

    [Fact]
    public async Task FindAsync_WhenEntityExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();
            CategorySearchArgs args = new CategorySearchArgs
            {
                SearchTerm = "Test",
                IsActive = true
            };

            List<Category>? categories = await _categoryRepository.FindAsync(args);

            Assert.NotNull(categories);
            Assert.Equal("Test Category 1", categories.First().Name);
        }
    }

    [Fact]
    public async Task FindAsync_WhenEntityDoesnotExist_ShouldReturnEmptyItems()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            ICategoryRepository _categoryRepository = _fixture.Container.GetInstance<ICategoryRepository>();
            CategorySearchArgs args = new CategorySearchArgs
            {
                SearchTerm = "Negative",
                IsActive = true
            };

            List<Category>? categories = await _categoryRepository.FindAsync(args);

            Assert.Empty(categories);
        }
    }
}
