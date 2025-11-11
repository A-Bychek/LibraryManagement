using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        public Task<Book> GetByIdAsync(long bookId, CancellationToken cancellationToken = default);
        public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
        public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Book book, CancellationToken cancellationToken = default);
        public Task<ICollection<Book>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Book> FindAsync(long bookId, BookSearchArgs args, CancellationToken cancellationToken = default);
    }

}
