using FluentAssertions;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Integration.Tests.Base;

namespace LibraryManagement.Integration.Tests.Application.Mappings;

public class CategoryMappingTests : AutoMapperTestBase
{
    [Fact]
    public void Borrowing_To_BorrowingDto_ShouldMapCorrectly()
    {
        Category parentCategory = new Category
        {
            CategoryId = 1,
            Name = "Test Parent Category",
            Description = "Test Parent Category Description",
            SortOrder = 1,
            IsActive = true
        };

        Category category = new Category
        {
            CategoryId = 2,
            Name = "Test Category",
            Description = "Test Description",
            ParentCategory = parentCategory,
            ParentCategoryId = 1,
            SortOrder = 1,
            IsActive = true
        };

        var categoryDto = _mapper.Map<CategoryDto>(category);

        categoryDto.Should().NotBeNull();
        categoryDto.CategoryId.Should().Be(category.CategoryId);
        categoryDto.Name.Should().Be(category.Name);
        categoryDto.Description.Should().Be(category.Description);
        categoryDto.ParentCategoryId.Should().Be(category.ParentCategoryId);
        categoryDto.ParentCategoryName.Should().Be(parentCategory.Name);
        categoryDto.SortOrder.Should().Be(category.SortOrder);
        categoryDto.IsActive.Should().Be(category.IsActive);
    }
}
