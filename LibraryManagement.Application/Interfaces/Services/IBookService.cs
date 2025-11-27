using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBookService
{
    public Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken = default);
    public Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs args, CancellationToken cancellationToken =default);
    public Task<BookDto> CreateBookAsync(CreateBookCommand command, CancellationToken cancellationToken = default);
    public Task<BookDto> UpdateBookAsync(UpdateBookCommand command, CancellationToken cancellationToken = default);
    public Task<DeleteBookDto> DeleteBookAsync(long bookId, CancellationToken cancellationToken = default);
}
