using AutoMapper;
using Grpc.Core;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Borrowings;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Contract.QueryModels.Borrowings;
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
        {
            BorrowBookCommand borrowBookCommand = _mapper.Map<BorrowBookCommand>(borrowBooкRequest);
            BorrowingDto borrowing = await _borrowingService.BorrowBookAsync(borrowBookCommand, context.CancellationToken);
            _logger.Information($"Book ID: {borrowBookCommand.BookId}, User ID: {borrowBookCommand.UserId}, " +
                $"days to return: {borrowBookCommand.DaysToReturn}");

            return _mapper.Map<BorrowingResponse>(borrowing);
        }        
    }

    public override async Task<BorrowingResponse> ReturnBook(ReturnBookRequest returnBookRequest, ServerCallContext context)
    {
        {
            ReturnBookCommand returnBookCommand = _mapper.Map<ReturnBookCommand>(returnBookRequest);
            BorrowingDto borrowing = await _borrowingService.ReturnBookAsync(returnBookCommand, context.CancellationToken);
            _logger.Information($"Borrowing entity has been returned: ID: {borrowing.BorrowingId}, " +
                $"borrow date: {borrowing.BorrowDate}, return date: {borrowing.ReturnDate}, fine amount: {borrowing.FineAmount}.");
            return _mapper.Map<BorrowingResponse>(borrowing);
        }        
    }

    public override async Task<BorrowingListResponse> GetUserBorrowings(UserBorrowingsRequest userBorrowingsRequest, ServerCallContext context)
    {
        {
            BorrowingSearchArgs borrowingSearchArgs = _mapper.Map<BorrowingSearchArgs>(userBorrowingsRequest);

            var borrowings = await _borrowingService.GetUserBorrowingsAsync(borrowingSearchArgs, context.CancellationToken);

            var mappedBorrowings = borrowings.Items.Select(_mapper.Map<BorrowingResponse>);

            var borrowingResponse = _mapper.Map<BorrowingListResponse>(borrowings);

            borrowingResponse.Borrowings.AddRange(mappedBorrowings);
            _logger.Information($"{borrowings.TotalCount} Borrowings have been found.");
            return borrowingResponse;
        }
    }

    public override async Task<BorrowingListResponse> GetOverdueBooks(OverdueBooksRequest overdueBooksRequest, ServerCallContext context)
    {
        {
            
            var overdueBooks = await _borrowingService.GetOverdueBooksAsync(context.CancellationToken);

            var mappedOverdueBooks = overdueBooks.Select(_mapper.Map<BorrowingResponse>);

            var borrowingResponse = _mapper.Map<BorrowingListResponse>(overdueBooksRequest, opt => opt.AfterMap((src, dest) => dest.TotalCount = overdueBooks.Count));

            borrowingResponse.Borrowings.AddRange(mappedOverdueBooks);
            _logger.Information($"{borrowingResponse.TotalCount} Borrowings have been found.");
            return borrowingResponse;

        }
    }
}
