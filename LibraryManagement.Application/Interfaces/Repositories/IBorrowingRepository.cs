using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories;

public interface IBorrowingRepository
{
    public Task<Borrowing?> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default);
    public Task<Borrowing> AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
    public Task<Borrowing> UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
    public Task<ICollection<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default);
}
