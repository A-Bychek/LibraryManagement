using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Contract.Commands.Category;
using LibraryManagement.Contract.QueryModels.Categories;

namespace LibraryManagement.Application.Interfaces.Services;

public interface ICategoryService
{
    public Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken = default);
    public Task<List<CategoryDto>> GetCategoriesAsync(CategorySearchArgs args, CancellationToken cancellationToken = default);
    public Task<List<CategoryDto>> GetCategoryTreeAsync(bool include_inactive, CancellationToken cancellationToken = default);
    public Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default);
    public Task<string> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken = default); // double-check this later
}
