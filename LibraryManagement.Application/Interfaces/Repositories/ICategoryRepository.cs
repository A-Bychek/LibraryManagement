using LibraryManagement.Contract.QueryModels.Categories;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    public Task<Category?> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default);
    public Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default);
    public Task<List<Category>> FindAsync(CategorySearchArgs args, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Category>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken = default);
}
