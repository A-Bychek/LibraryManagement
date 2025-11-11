using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.QueryModels.Categories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        public Task<Category> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Category> FindAsync(long categoryId, CategorySearchArgs args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
