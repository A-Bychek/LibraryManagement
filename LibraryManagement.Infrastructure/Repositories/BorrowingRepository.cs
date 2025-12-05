using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BorrowingRepository: IBorrowingRepository
{
    private readonly LibraryManagementDbContext _context;

    public BorrowingRepository(LibraryManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Borrowing?> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings
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

    public async Task<ICollection<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings.Include(x => x.Book).ToListAsync(cancellationToken);
    }
}
