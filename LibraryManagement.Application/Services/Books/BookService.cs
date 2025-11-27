using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Books;

public class BookService : IBookService
{
    private IBookRepository _bookRepository { get; set; } = null!;
    private IMapper _mapper { get; set; } = null!;
    private IValidator<CreateBookCommand> _createBookCommandValidator { get; set; }
    private IValidator<UpdateBookCommand> _updateBookCommandValidator { get; set; }
    private IValidator<BookSearchArgs> _bookSearchArgsValidator { get; set; }
    public BookService(
        IBookRepository bookRepository,
        IMapper mapper,
        IValidator<CreateBookCommand> createBookCommandValidator,
        IValidator<UpdateBookCommand> updateBookCommandValidator,
        IValidator<BookSearchArgs> bookSearchArgsValidator
        )
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _createBookCommandValidator = createBookCommandValidator;
        _updateBookCommandValidator = updateBookCommandValidator;
        _bookSearchArgsValidator = bookSearchArgsValidator;
    }

    public async Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken)
            ?? throw new NotFoundException($"Can't find the book with {bookId} bookId!");
        return _mapper.Map<BookDto>(book);
    }

    public async Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs bookSearchArgs, CancellationToken cancellationToken = default)
    {
        await _bookSearchArgsValidator.ValidateAndThrowAsync(bookSearchArgs, cancellationToken);
        var books = await _bookRepository.FindAsync(bookSearchArgs, cancellationToken);

        var mappedBooks = books.Items.Select(_mapper.Map<BookDto>);

        return PagedResult<BookDto>.Create(mappedBooks, books.TotalCount, books.PageNumber, books.PageSize); // re-check this

    }

    public async Task<BookDto> CreateBookAsync(CreateBookCommand createBookCommand, CancellationToken cancellationToken = default)
    {
        await _createBookCommandValidator.ValidateAndThrowAsync(createBookCommand, cancellationToken);
        var book = _mapper.Map<Book>(createBookCommand);
        await _bookRepository.AddAsync(book, cancellationToken);
        return await GetBookAsync(book.BookId, cancellationToken);
    }

    public async Task<BookDto> UpdateBookAsync(UpdateBookCommand updateBookCommand, CancellationToken cancellationToken = default)
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

    public async Task<DeleteBookDto> DeleteBookAsync(long bookId, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken)
            ?? throw new NotFoundException($"Successfully removed the book with {bookId} bookId.");

        await _bookRepository.DeleteAsync(book, cancellationToken);
        return new DeleteBookDto
        {
            Success = true,
            Message = $"Successfully removed the book with {bookId} bookId."
        };
    }
}
