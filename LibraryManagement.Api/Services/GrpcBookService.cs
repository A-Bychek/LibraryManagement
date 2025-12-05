using AutoMapper;
using Grpc.Core;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using Serilog;

namespace LibraryManagement.Api.Services;

public class GrpcBookService : BookService.BookServiceBase
{
    private readonly IBookService _BookService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public GrpcBookService(IBookService BookService, IMapper mapper)
    {
        _BookService = BookService;
        _mapper = mapper;
        _logger = Log.Logger;
    }

    public override async Task<BookGetResponse> GetBook(BookGetRequest request, ServerCallContext context)
    {
        BookDto Book = await _BookService.GetBookAsync(request.BookId, context.CancellationToken);
        _logger.Information($"Book ID: {Book.BookId}, Title: {Book.Title}, Author: {Book.AuthorName}, Pages: {Book.PageCount}");

        return new BookGetResponse
        {
            Book = _mapper.Map<BookResponse>(Book)
        };
    }

    public override async Task<BookResponse> CreateBook(CreateBookRequest request, ServerCallContext context)
    {
        var createBookCommand = _mapper.Map<CreateBookCommand>(request);
        var Book = await _BookService.CreateBookAsync(createBookCommand, context.CancellationToken);
        _logger.Information($"Book entity has been created: ID: {Book.BookId}," +
            $"Title: {Book.Title}, Author: {Book.AuthorName}, Pages: {Book.PageCount}," +
            $"Description: {Book.Description}");
        return _mapper.Map<BookResponse>(Book);
    }

    public override async Task<BookListResponse> GetBooks(BookSearchRequest request, ServerCallContext context)
    {
        var searchBookCommand = _mapper.Map<BookSearchArgs>(request);

        var books = await _BookService.GetBooksAsync(searchBookCommand, context.CancellationToken);

        var mappedBooks = books.Items.Select(_mapper.Map<BookResponse>);

        var bookResponse = _mapper.Map<BookListResponse>(books, opt => opt.AfterMap((src, dest) => dest.Books.AddRange(mappedBooks)));

        _logger.Information($"{books.TotalCount} Books have been found.");
        return bookResponse;
    }

    public override async Task<BookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
    {
        var updateBookCommand = _mapper.Map<UpdateBookCommand>(request);
        var Book = await _BookService.UpdateBookAsync(updateBookCommand, context.CancellationToken);
        _logger.Information($"Book entity has been updated: ID: {Book.BookId}," +
            $"Title: {Book.Title}, Description: {Book.Description}, Author: {Book.AuthorName}, " +
            $"Pages: {Book.PageCount}");
        return _mapper.Map<BookResponse>(Book);
    }

    public override async Task<DeleteResponse> DeleteBook(BookDeleteRequest request, ServerCallContext context)
    {
        DeleteBookDto deletedBook = await _BookService.DeleteBookAsync(request.BookId, context.CancellationToken);
        return _mapper.Map<DeleteResponse>(deletedBook);
    }
}
