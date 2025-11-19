using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBorrowingService
    {
        public Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand command, CancellationToken cancellationToken);
        public Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand command, CancellationToken cancellationToken);
        public Task<List<BorrowingDto>> GetUserBorrowingsAsync(long userId, CancellationToken cancellationToken);
        public Task<List<BorrowingDto>> GetOverdueBooksAsync(CancellationToken cancellationToken);
        public Task<double> CalculateFineAsync(long borrowingId, CancellationToken cancellationToken);
    }
}
