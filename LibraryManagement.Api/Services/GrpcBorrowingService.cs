using AutoMapper;
using Grpc.Core;
using FluentValidation;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Borrowings;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Contract.QueryModels.Borrowings;
using LibraryManagement.Shared.Exceptions;
using Serilog;

namespace LibraryManagement.Api.Services;

public class GrpcBorrowingService : BorrowingService.BorrowingServiceBase
{
    private readonly IBorrowingService _borrowingService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public GrpcBorrowingService(IBorrowingService borrowingService, IMapper mapper)
    {
        _borrowingService = borrowingService;
        _mapper = mapper;
        _logger = Log.Logger;
    }

    public override async Task<BorrowingResponse> BorrowBook(BorrowBookRequest borrowBooкRequest, ServerCallContext context)
    {
        try
        {
            BorrowBookCommand borrowBookCommand = _mapper.Map<BorrowBookCommand>(borrowBooкRequest);
            BorrowingDto borrowing = await _borrowingService.BorrowBookAsync(borrowBookCommand, context.CancellationToken);
            _logger.Information($"Book ID: {borrowBookCommand.BookId}, User ID: {borrowBookCommand.UserId}, " +
                $"days to return: {borrowBookCommand.DaysToReturn}");

            return _mapper.Map<BorrowingResponse>(borrowing);
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

    public override async Task<BorrowingResponse> ReturnBook(ReturnBookRequest returnBookRequest, ServerCallContext context)
    {
        try
        {
            ReturnBookCommand returnBookCommand = _mapper.Map<ReturnBookCommand>(returnBookRequest);
            BorrowingDto borrowing = await _borrowingService.ReturnBookAsync(returnBookCommand, context.CancellationToken);
            _logger.Information($"Borrowing entity has been returned: ID: {borrowing.BorrowingId}, " +
                $"borrow date: {borrowing.BorrowDate}, return date: {borrowing.ReturnDate}, fine amount: {borrowing.FineAmount}.");
            return _mapper.Map<BorrowingResponse>(borrowing);
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

    public override async Task<BorrowingListResponse> GetUserBorrowings(UserBorrowingsRequest userBorrowingsRequest, ServerCallContext context)
    {
        try
        {
            BorrowingSearchArgs borrowingSearchArgs = _mapper.Map<BorrowingSearchArgs>(userBorrowingsRequest);

            var borrowings = await _borrowingService.GetUserBorrowingsAsync(borrowingSearchArgs, context.CancellationToken);

            var mappedBorrowings = borrowings.Items.Select(_mapper.Map<BorrowingResponse>);

            var borrowingResponse = _mapper.Map<BorrowingListResponse>(borrowings);

            borrowingResponse.Borrowings.AddRange(mappedBorrowings);
            _logger.Information($"{borrowings.TotalCount} Borrowings have been found.");
            return borrowingResponse;
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

    public override async Task<BorrowingListResponse> GetOverdueBooks(OverdueBooksRequest overdueBooksRequest, ServerCallContext context)
    {
        try
        {
            
            var overdueBooks = await _borrowingService.GetOverdueBooksAsync(context.CancellationToken);

            var mappedOverdueBooks = overdueBooks.Select(_mapper.Map<BorrowingResponse>);

            var borrowingResponse = _mapper.Map<BorrowingListResponse>(overdueBooksRequest, opt => opt.AfterMap((src, dest) => dest.TotalCount = overdueBooks.Count));

            borrowingResponse.Borrowings.AddRange(mappedOverdueBooks);
            _logger.Information($"{borrowingResponse.TotalCount} Borrowings have been found.");
            return borrowingResponse;

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
}
