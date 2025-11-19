using LibraryManagement.Application.Commands.Category;
using LibraryManagement.Application.DTOs.Categories;
using LibraryManagement.Application.QueryModels.Categories;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken);
        public Task<List<CategoryDto>> GetCategoriesAsync(CategorySearchArgs args, CancellationToken cancellationToken);
        public Task<List<CategoryDto>> GetCategoryTreeAsync(CancellationToken cancellationToken);
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand command, CancellationToken cancellationToken);
        public Task<string> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken); // double-check this later
    }
}
