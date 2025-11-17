using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBookService
    {
        public Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken);
        public Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs args, CancellationToken cancellationToken);
        public Task<BookDto> CreateBookAsync(CreateBookCommand command, CancellationToken cancellationToken);
        public Task<BookDto> UpdateBookAsync(UpdateBookCommand command, CancellationToken cancellationToken);
        public Task<BookDto> DeleteBookAsync(long bookId, CancellationToken cancellationToken);
        public Task<BorrowingStatus> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken);
    }
}
