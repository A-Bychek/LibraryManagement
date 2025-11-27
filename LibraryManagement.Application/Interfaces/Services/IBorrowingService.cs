using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Contract.QueryModels.Borrowings;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBorrowingService
{
    public Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand command, CancellationToken cancellationToken = default);
    public Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand command, CancellationToken cancellationToken = default);
    public Task<PagedResult<BorrowingDto>> GetUserBorrowingsAsync(BorrowingSearchArgs borrowingSearchArgs, CancellationToken cancellationToken = default);
    public Task<List<BorrowingDto>> GetOverdueBooksAsync(CancellationToken cancellationToken = default);
    public Task<double> CalculateFineAsync(long borrowingId, CancellationToken cancellationToken = default);
}
