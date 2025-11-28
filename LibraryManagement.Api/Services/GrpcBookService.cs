using AutoMapper;
using FluentValidation;
using Grpc.Core;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Commands.Book;
using LibraryManagement.Contract.QueryModels.Books;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Exceptions;
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
        try
        {
            BookDto Book = await _BookService.GetBookAsync(request.BookId, context.CancellationToken);
            _logger.Information($"Book ID: {Book.BookId}, Title: {Book.Title}, Author: {Book.AuthorName}, Pages: {Book.PageCount}");

            return new BookGetResponse
            {
                Book = _mapper.Map<BookResponse>(Book)
            };
        }

        catch (NotFoundException exc)
        {
            _logger.Error($"Not found: {exc.Message}");
            throw new RpcException(new Status(StatusCode.NotFound, exc.Message));
        }
        catch (Exception exc)
        {
            _logger.Error($"Unknown issue occured: {exc.Message}");
            throw new RpcException(new Status(StatusCode.Unknown, $"Unknown issue: Message => {exc.Message}," +
                $"Source => {exc.Source}, Data => {exc.Data}"));
        }
    }

    public override async Task<BookResponse> CreateBook(CreateBookRequest request, ServerCallContext context)
    {
        try
        {
            var createBookCommand = _mapper.Map<CreateBookCommand>(request);
            var Book = await _BookService.CreateBookAsync(createBookCommand, context.CancellationToken);
            _logger.Information($"Book entity has been created: ID: {Book.BookId}," +
                $"Title: {Book.Title}, Author: {Book.AuthorName}, Pages: {Book.PageCount}," +
                $"Description: {Book.Description}");
            return _mapper.Map<BookResponse>(Book);
        }
        catch (ValidationException exc)
        {
            _logger.Error($"Validation failed: {exc.Message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, exc.Message));
        }
        catch (Exception exc)
        {
            _logger.Error($"Unknown issue: {exc.Message}.");
            throw new RpcException(new Status(StatusCode.Unknown, $"Unknown issue: {exc.Message}."));
        }
    }

    public override async Task<BookListResponse> GetBooks(BookSearchRequest request, ServerCallContext context)
    {
        try
        {
            var searchBookCommand = _mapper.Map<BookSearchArgs>(request);

            var books = await _BookService.GetBooksAsync(searchBookCommand, context.CancellationToken);

            var mappedBooks = books.Items.Select(_mapper.Map<BookResponse>);

            var bookResponse = _mapper.Map<BookListResponse>(books, opt => opt.AfterMap((src, dest) => dest.Books.AddRange(mappedBooks)));

            _logger.Information($"{books.TotalCount} Books have been found.");
            return bookResponse;
        }
        catch (ValidationException exc)
        {
            _logger.Error($"Validation failed: {exc.Message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, exc.Message));
        }
        catch (Exception exc)
        {
            _logger.Error($"Unknown issue: {exc.Message}");
            throw new RpcException(new Status(StatusCode.Unknown, $"Unknown issue: {exc.Message}."));
        }
    }

    public override async Task<BookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
    {
        try
        {
            var updateBookCommand = _mapper.Map<UpdateBookCommand>(request);
            var Book = await _BookService.UpdateBookAsync(updateBookCommand, context.CancellationToken);
            _logger.Information($"Book entity has been updated: ID: {Book.BookId}," +
                $"Title: {Book.Title}, Description: {Book.Description}, Author: {Book.AuthorName}, " +
                $"Pages: {Book.PageCount}");
            return _mapper.Map<BookResponse>(Book);
        }
        catch (ValidationException exc)
        {
            _logger.Error($"Validation failed: {exc.Message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, exc.Message));
        }
        catch (Exception exc)
        {
            _logger.Error($"Unknown issue: {exc.Message}");
            throw new RpcException(new Status(StatusCode.Unknown, $"Unknown issue: {exc.Message}."));
        }
    }

    public override async Task<DeleteResponse> DeleteBook(BookDeleteRequest request, ServerCallContext context)
    {
        try
        {
            DeleteBookDto deletedBook = await _BookService.DeleteBookAsync(request.BookId, context.CancellationToken);
            return _mapper.Map<DeleteResponse>(deletedBook);
        }

        catch (NotFoundException exc)
        {
            _logger.Error($"Not found: {exc.Message}");
            throw new RpcException(new Status(StatusCode.NotFound, exc.Message));
        }
        catch (Exception exc)
        {
            _logger.Error($"Unknown issue occured: {exc.Message}");
            throw new RpcException(new Status(StatusCode.Unknown, $"Unknown issue: Message => {exc.Message}," +
                $"Source => {exc.Source}, Data => {exc.Data}"));
        }
    }
}
