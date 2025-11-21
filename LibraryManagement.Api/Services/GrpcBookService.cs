using AutoMapper;
using Grpc.Core;
using LibraryManagement.Application.Commands.Book;
using LibraryManagement.Application.DTOs.Books;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Books;
using LibraryManagement.Contract.Books;
using LibraryManagement.Shared.Exceptions;
using Serilog;
using System.ComponentModel.DataAnnotations;


namespace LibraryManagement.Api.Services
{
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
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed."));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue: {exc.Message}, {exc.InnerException}, {exc.GetBaseException}");
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown issue."));
            }
        }

        public override async Task<BookListResponse> GetBooks(BookSearchRequest request, ServerCallContext context)
        {
            try
            {
                var searchBookCommand = _mapper.Map<BookSearchArgs>(request);

                var Books = await _BookService.GetBooksAsync(searchBookCommand, context.CancellationToken);

                var mappedBooks = Books.Items.Select(_mapper.Map<BookResponse>);

                var BookResponse = new BookListResponse
                {
                    TotalCount = Books.TotalCount,
                    PageNumber = Books.PageNumber,
                    PageSize = Books.PageSize
                };

                BookResponse.Books.AddRange(mappedBooks);
                _logger.Information($"{Books.TotalCount} Books have been found.");
                return BookResponse;
            }
            catch (ValidationException exc)
            {
                _logger.Error($"Validation failed: {exc.Message}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed."));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue: {exc.Message}");
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown issue."));
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
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed."));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue: {exc.Message}");
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown issue."));
            }
        }

    public override async Task<DeleteResponse> DeleteBook(BookDeleteRequest request, ServerCallContext context)
        {
            try
            {
                await _BookService.DeleteBookAsync(request.BookId, context.CancellationToken);

                return new DeleteResponse
                {
                    Success = true,
                    Message = $"Successfully removed {request.BookId} bookId."
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
    }
}
