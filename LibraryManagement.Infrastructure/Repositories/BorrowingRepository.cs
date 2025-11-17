using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowingRepository: IBorrowingRepository
    {
        private readonly LibraryManagementDbContext _context;

        public BorrowingRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }
        public async Task<Borrowing> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default)
        {
            return await _context.Borrowings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BorrowingId == borrowingId, cancellationToken);
        }
        public async Task<Borrowing> AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            await _context.Borrowings.AddAsync(borrowing, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return borrowing;
        }
        public async Task<Borrowing> UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            _context.Borrowings.Update(borrowing);
            await _context.SaveChangesAsync(cancellationToken);
            return borrowing;
        }
        public async Task<Borrowing> DeleteAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            _context.Borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }
        public async Task<ICollection<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Borrowings.ToListAsync(cancellationToken);
        }
        public async Task<PagedResult<Borrowing>> FindAsync(BorrowingSearchArgs borrowingSearchArgs, CancellationToken cancellationToken = default)
        {
            var query = _context.Borrowings
                .Include(x => x.Book)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(borrowingSearchArgs.SearchTerm))
            {
                var searchTerm = borrowingSearchArgs.SearchTerm.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Status.ToString(), $"%{searchTerm}%"));
            }

            if (borrowingSearchArgs.IsActive.HasValue)
            {
                query = query.Where(x => x.Status.ToString() == borrowingSearchArgs.IsActive.Value.ToString());
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((borrowingSearchArgs.PageNumber - 1) * borrowingSearchArgs.PageSize)
                .Take(borrowingSearchArgs.PageSize)
                .ToListAsync(cancellationToken);

            return PagedResult<Borrowing>.Create(items, totalCount, borrowingSearchArgs.PageNumber, borrowingSearchArgs.PageSize);
        }
    }
}
