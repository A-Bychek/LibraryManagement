using LibraryManagement.Application.QueryModels.Categories;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<Category> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default);
        public Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default);
        // public Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken = default);
        // public Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken = default);
        public Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Category> FindAsync(long categoryId, CategorySearchArgs args, CancellationToken cancellationToken = default);
    }

}

