using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        public async Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<List<BookDto>> GetBooksAsync(BookSearchArgs bookSearchArgs, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<BookDto> CreateBookAsync(CreateBookCommand createBookCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<BookDto> UpdateBookAsync(UpdateBookCommand updateBookCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteBookAsync(long bookId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<BookDto> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
