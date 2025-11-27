using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Contract.QueryModels.Categories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly LibraryManagementDbContext _context;

    public CategoryRepository(LibraryManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Category> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(x => x.Books)
            .FirstOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);
    }

    public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }
  
    public async Task<IEnumerable<Category>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken = default)
    {
        var categories = await _context.Categories
            .Include(x => x.Books)
            .Where(x => x.IsActive == includeInactive)
            .AsNoTracking()
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
        return categories;
    }

    public async Task<List<Category>> FindAsync(CategorySearchArgs categorySearchArgs, CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.Books)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(categorySearchArgs.SearchTerm))
        {
            var searchTerm = categorySearchArgs.SearchTerm.ToLower();
            query = query.Where(c =>
                EF.Functions.Like(c.Name, $"%{searchTerm}%") ||
                EF.Functions.Like(c.Description, $"%{searchTerm}%"));
        }

        if (categorySearchArgs.ParentCategoryId.HasValue)
        {
            query = query.Where(c => c.ParentCategoryId == categorySearchArgs.ParentCategoryId.Value);
        }

        if (categorySearchArgs.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == categorySearchArgs.IsActive.Value);
        }

        List<Category> result = await query
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return result;
    }
}
