using LibraryManagement.Application.Commands.Category;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public async Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken)
        { 
            throw new NotImplementedException(); 
        }
        public async Task<List<CategoryDto>> GetCategoriesAsync(CategorySearchArgs categorySearchArgs, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CategoryDto>> GetCategoryTreeAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<string> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
