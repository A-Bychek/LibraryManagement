using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository: IBookRepository
    {
        public Task<Book> GetByIdAsync(long bookId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Book book, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<ICollection<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Book> FindAsync(long bookId, BookSearchArgs args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
