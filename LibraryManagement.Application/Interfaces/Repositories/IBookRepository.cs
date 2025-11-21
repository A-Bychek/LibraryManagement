using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        public Task<Book?> GetByIdAsync(long bookId, CancellationToken cancellationToken = default);
        public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
        public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default);
        public Task<Book> DeleteAsync(Book book, CancellationToken cancellationToken = default);
        public Task<ICollection<Book>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<PagedResult<Book>> FindAsync(BookSearchArgs bookSearchArgs, CancellationToken cancellationToken = default);
    }

}
