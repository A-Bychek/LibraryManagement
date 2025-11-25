using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Repositories;

public interface IBorrowingRepository
{
    public Task<Borrowing> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default);
    public Task<Borrowing> AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
    public Task<Borrowing> UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
    public Task<Borrowing> DeleteAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
    public Task<ICollection<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task SaveAsync(CancellationToken cancellationToken = default);
}
