using LibraryManagement.Application.Commands.Category;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.QueryModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
