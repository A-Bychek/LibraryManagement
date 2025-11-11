using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.QueryModels.Books;
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
        public Task<List<BookDto>> GetBooksAsync(BookSearchArgs args, CancellationToken cancellationToken);
        public Task<BookDto> CreateBookAsync(CreateBookCommand command, CancellationToken cancellationToken);
        public Task<BookDto> UpdateBookAsync(UpdateBookCommand command, CancellationToken cancellationToken);
        public Task<bool> DeleteBookAsync(long bookId, CancellationToken cancellationToken);
        public Task<BookDto> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken);
    }
}
