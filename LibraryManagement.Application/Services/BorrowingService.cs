using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowing;
using LibraryManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class BorrowingService: IBorrowingService
    {
        public async Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand borrowBookCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand returnBookCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<List<BorrowingDto>> GetUserBorrowingsAsync(long userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<List<BorrowingDto>> GetOverdueBooksAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<double> CalculateFineAsync(long borrowingid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
