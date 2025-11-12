using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly LibraryManagementDbContext _context;

        public BookRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }
        public async Task<Book> GetByIdAsync(long bookId, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BookId == bookId, cancellationToken);
        }
        public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
        {
            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return book;
        }
        public async Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync(cancellationToken);
            return book;
        }
        public async Task<Book> DeleteAsync(Book book, CancellationToken cancellationToken = default)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<ICollection<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Books.ToListAsync(cancellationToken);
        }
        public async Task<PagedResult<Book>> FindAsync(BookSearchArgs bookSearchArgs, CancellationToken cancellationToken = default)
        {
            var query = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(bookSearchArgs.SearchTerm))
            {
                var searchTerm = bookSearchArgs.SearchTerm.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(x.ISBN, $"%{searchTerm}%"));
            }

            if (bookSearchArgs.AuthorId.HasValue)
            {
                query = query.Where(x => x.AuthorId == bookSearchArgs.AuthorId.Value);
            }

            if (bookSearchArgs.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == bookSearchArgs.CategoryId.Value);
            }

            if (bookSearchArgs.IsAvailable.HasValue)
            {
                query = query.Where(x => x.IsAvailable == bookSearchArgs.IsAvailable.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((bookSearchArgs.PageNumber - 1) * bookSearchArgs.PageSize)
                .Take(bookSearchArgs.PageSize)
                .ToListAsync(cancellationToken);

            return PagedResult<Book>.Create(items, totalCount, bookSearchArgs.PageNumber, bookSearchArgs.PageSize);
        }
    }
}
