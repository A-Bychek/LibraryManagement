using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Books
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository { get; set; } = null!;
        private IMapper _mapper { get; set; } = null!;
        private IValidator<CreateBookCommand> _createBookCommandValidator { get; set; }
        private IValidator<UpdateBookCommand> _updateBookCommandValidator { get; set; }
        public BookService(
            IBookRepository bookRepository,
            IMapper mapper,
            IValidator<CreateBookCommand> createBookCommandValidator,
            IValidator<UpdateBookCommand> updateBookCommandValidator
            )
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _createBookCommandValidator = createBookCommandValidator;
            _updateBookCommandValidator = updateBookCommandValidator;
        }
        public async Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs bookSearchArgs, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.FindAsync(bookSearchArgs, cancellationToken);

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
            await _createBookCommandValidator.ValidateAndThrowAsync(createBookCommand, cancellationToken);
            var book = new Book()
            {
                Title = createBookCommand.Title,
                ISBN = createBookCommand.ISBN,
                Description = createBookCommand.Description,
                AuthorId = createBookCommand.AuthorId,
                CategoryId = createBookCommand.CategoryId,
                PublishedDate = DateTime.Parse(createBookCommand.PublishedDate),
                PageCount = createBookCommand.PageCount
            };
            await _bookRepository.AddAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(UpdateBookCommand updateBookCommand, CancellationToken cancellationToken)
        {
            await _updateBookCommandValidator.ValidateAndThrowAsync(updateBookCommand, cancellationToken);
            var book = await _bookRepository.GetByIdAsync(updateBookCommand.BookId, cancellationToken) ?? throw new NotFoundException($"Can't find a {updateBookCommand.BookId} book!");
            book.Title = updateBookCommand.Title ?? book.Title;
            book.Description = updateBookCommand.Description ?? book.Description;
            book.CategoryId = updateBookCommand.CategoryId ?? book.CategoryId;
            book.PublishedDate = updateBookCommand.PublishedDate is not null ? DateTime.Parse(updateBookCommand.PublishedDate) : book.PublishedDate;
            book.PageCount = updateBookCommand.PageCount ?? book.PageCount;

            await _bookRepository.UpdateAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> DeleteBookAsync(long bookId, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
            await _bookRepository.DeleteAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BorrowingStatus> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // return _context.Borrowings.Where(x => x.BookId == bookId).FirstOrDefault().Status; // re-check this
        }
    }
}
