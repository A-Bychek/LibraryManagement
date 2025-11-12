using AutoMapper;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Shared;
namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _context { get; set; } = null!;
        private IMapper _mapper { get; set; } = null!;
        public BookService(IBookRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken)
        {
            var book = await _context.GetByIdAsync(bookId, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs bookSearchArgs, CancellationToken cancellationToken)
        {
            var books = await _context.FindAsync(bookSearchArgs, cancellationToken);

            List<BookDto> mappedBooks = new List<BookDto>();
            foreach (var book in books)
            {
                BookDto mappedBook = _mapper.Map<BookDto>(book);
                mappedBooks.Append(mappedBook);
            }

            return PagedResult<BookDto>.Create(mappedBooks, books.TotalCount, books.PageNumber, books.PageSize); // re-check this

        }
        public async Task<BookDto> CreateBookAsync(CreateBookCommand createBookCommand, CancellationToken cancellationToken)
        {
            var book = new Book()
            {
                Title = createBookCommand.Title,
                ISBN = createBookCommand.ISBN,
                Description = createBookCommand.Description,
                AuthorId = createBookCommand.AuthorId,
                CategoryId = createBookCommand.CategoryId,
                PublishedDate = Convert.ToDateTime(createBookCommand.PublishedDate),
                PageCount = createBookCommand.PageCount
            };
            await _context.AddAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(UpdateBookCommand updateBookCommand, CancellationToken cancellationToken)
        {
            var book = await _context.GetByIdAsync(updateBookCommand.BookId, cancellationToken) ?? throw new Exception($"Can't find a {updateBookCommand.BookId} book!");
            book.Title = updateBookCommand.Title ?? book.Title;
            book.Description = updateBookCommand.Description ?? book.Description;
            book.CategoryId = updateBookCommand.CategoryId ?? book.CategoryId;
            book.PublishedDate = updateBookCommand.PublishedDate is not null ? Convert.ToDateTime(updateBookCommand.PublishedDate) : book.PublishedDate;
            book.PageCount = updateBookCommand.PageCount ?? book.PageCount;

            await _context.UpdateAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> DeleteBookAsync(long bookId, CancellationToken cancellationToken)
        {
            var book = await _context.GetByIdAsync(bookId, cancellationToken);
            await _context.DeleteAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BorrowingStatus> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // return _context.Borrowings.Where(x => x.BookId == bookId).FirstOrDefault().Status; // re-check this
        }
    }
}
