using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBorrowingService
{
    public Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand command, CancellationToken cancellationToken);
    public Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand command, CancellationToken cancellationToken);
    public Task<PagedResult<BorrowingDto>> GetUserBorrowingsAsync(BorrowingSearchArgs borrowingSearchArgs, CancellationToken cancellationToken);
    public Task<List<BorrowingDto>> GetOverdueBooksAsync(CancellationToken cancellationToken);
    public Task<double> CalculateFineAsync(long borrowingId, CancellationToken cancellationToken);
}
