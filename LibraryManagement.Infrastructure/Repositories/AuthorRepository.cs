using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Shared;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class AuthorRepository: IAuthorRepository
    {
        private readonly LibraryManagementDbContext _context;

        public AuthorRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }
        public async Task<Author?> GetByIdAsync(long authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(x => x.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AuthorId == authorId, cancellationToken);
        }
        public async Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default)
        {
            await _context.Authors.AddAsync(author, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return author;
        }
        public async Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken = default)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync(cancellationToken);
            return author;
        }
        public async Task<Author> DeleteAsync(Author author, CancellationToken cancellationToken = default)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return author;
        }
        public async Task<ICollection<Author>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors.ToListAsync(cancellationToken);
        }
        public async Task<PagedResult<Author>> FindAsync(AuthorSearchArgs authorSearchArgs, CancellationToken cancellationToken = default)
        {
            var query = _context.Authors
                .Include(x => x.Books)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(authorSearchArgs.SearchTerm))
            {
                var searchTerm = authorSearchArgs.SearchTerm.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.FirstName, $"%{searchTerm}%") || EF.Functions.Like(x.LastName, $"%{searchTerm}%"));
            }

            if (authorSearchArgs.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == authorSearchArgs.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Skip((authorSearchArgs.PageNumber - 1) * authorSearchArgs.PageSize)
                .Take(authorSearchArgs.PageSize)
                .ToListAsync(cancellationToken);

            return PagedResult<Author>.Create(items, totalCount, authorSearchArgs.PageNumber, authorSearchArgs.PageSize);
        }
    }
}
