using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Borrowings;

public class BorrowingService: IBorrowingService
{
    private const double dailyFine = 1;

    private IBorrowingRepository _borrowingRepository;
    private IBookRepository _bookRepository;
    private IMapper _mapper;
    private IValidator<BorrowBookCommand> _borrowBookCommandValidator;
    private IValidator<ReturnBookCommand> _returnBookCommandValidator;

    public BorrowingService(
        IBorrowingRepository borrowingRepository,
        IBookRepository bookRepository,
        IMapper mapper,
        IValidator<BorrowBookCommand> borrowBookCommandValidator,
        IValidator<ReturnBookCommand> returnBookCommandValidator)
    {
        _borrowingRepository = borrowingRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _borrowBookCommandValidator = borrowBookCommandValidator;
        _returnBookCommandValidator = returnBookCommandValidator;
    }

    public async Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand borrowBookCommand, CancellationToken cancellationToken = default)
    {
        await _borrowBookCommandValidator.ValidateAndThrowAsync(borrowBookCommand, cancellationToken);

        Book book = await _bookRepository.GetByIdAsync(borrowBookCommand.BookId, cancellationToken);

        if (!book.IsAvailable)
        {
            throw new NotAvailableException($"Can't borrow the {book.Title} book");
        }

        DateTime borrowDate = DateTime.UtcNow;
        DateTime dueDate = borrowDate.AddDays(borrowBookCommand.DaysToReturn <= 0 ? 14 : borrowBookCommand.DaysToReturn);

        var borrowing = new Borrowing(
            borrowBookCommand.BookId, 
            borrowBookCommand.UserId,   
            borrowDate,
            dueDate, 
            null, 
            BorrowingStatus.Active
            );

        book.IsAvailable = false;

        await _borrowingRepository.AddAsync(borrowing, cancellationToken);
        await _bookRepository.UpdateAsync(book, cancellationToken);

        return _mapper.Map<Borrowing, BorrowingDto>(borrowing);
    }

    public async Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand returnBookCommand, CancellationToken cancellationToken = default)
    {
        await _returnBookCommandValidator.ValidateAndThrowAsync(returnBookCommand, cancellationToken);

        await UpdateStatuses(cancellationToken);

        var borrowing = await _borrowingRepository.GetByIdAsync(returnBookCommand.BorrowingId, cancellationToken)
            ?? throw new NotFoundException($"Can't find the {returnBookCommand.BorrowingId} borrowing entity!");

        if (borrowing.Status == BorrowingStatus.Active || borrowing.Status == BorrowingStatus.Overdue)
        {
            borrowing.Status = BorrowingStatus.Returned;
            borrowing.ReturnDate = DateTime.UtcNow;

            var book = await _bookRepository.GetByIdAsync(borrowing.BookId, cancellationToken)
                   ?? throw new NotFoundException($"Can't find the {borrowing.BookId} book!");
            book.IsAvailable = true;
            
            await _borrowingRepository.UpdateAsync(borrowing, cancellationToken);
            await _bookRepository.UpdateAsync(book, cancellationToken);   
        }
        var fineAmount = CalculateFineAsync(borrowing.BorrowingId, cancellationToken).Result;
        return _mapper.Map<Borrowing, BorrowingDto>(borrowing, opt => opt.AfterMap((src, dest) => dest.FineAmount = fineAmount));
    }

    public async Task<PagedResult<BorrowingDto>> GetUserBorrowingsAsync(
        BorrowingSearchArgs borrowingSearchArgs,
        CancellationToken cancellationToken = default
        )
    {
        await UpdateStatuses(cancellationToken);

        var userBorrowings =  _borrowingRepository.GetAllAsync(cancellationToken)
            .Result
            .Where(b => b.UserId == borrowingSearchArgs.UserId && b.Status == borrowingSearchArgs.Status);

        var mappedUserBorrowings = new List<BorrowingDto>();
        foreach (var userBorrowing in userBorrowings)
        {
            var fineAmount = CalculateFineAsync(userBorrowing.BorrowingId, cancellationToken).Result;
            var mappedUserBorrowing = _mapper.Map<Borrowing, BorrowingDto>(userBorrowing, opt => opt.AfterMap((src, dest) => dest.FineAmount = fineAmount));

            mappedUserBorrowings.Add(_mapper.Map<Borrowing, BorrowingDto>(userBorrowing, opt => opt.AfterMap((src, dest) => dest.FineAmount = fineAmount)));
        }

        return PagedResult<BorrowingDto>.Create(
            mappedUserBorrowings,
            mappedUserBorrowings.Count(),
            borrowingSearchArgs.PageNumber,
            borrowingSearchArgs.PageSize
            );
    }

    public async Task<List<BorrowingDto>> GetOverdueBooksAsync(CancellationToken cancellationToken = default)
    {
        await UpdateStatuses(cancellationToken);
        List<Borrowing> overdueBooks = _borrowingRepository.GetAllAsync(cancellationToken)
            .Result
            .Where(b => b.Status == BorrowingStatus.Overdue).ToList();
        var mappedBorrowings = new List<BorrowingDto>();
        foreach (var overdueBook in overdueBooks)
        {
            var fineAmount = CalculateFineAsync(overdueBook.BorrowingId, cancellationToken).Result;
            mappedBorrowings.Add(_mapper.Map<Borrowing, BorrowingDto>(overdueBook, opt => opt.AfterMap((src, dest) => dest.FineAmount = fineAmount)));
        }
        return mappedBorrowings;
    }

    public async Task<double> CalculateFineAsync(long borrowingid, CancellationToken cancellationToken = default)
    {
        var borrowing = await _borrowingRepository.GetByIdAsync(borrowingid, cancellationToken);
        if (borrowing.DueDate < DateTime.UtcNow)
        {
            return dailyFine * (DateTime.UtcNow - borrowing.DueDate).Days;
        }
        return 0;
    }

    public async Task UpdateStatuses(CancellationToken cancellationToken = default)
    {
        var borrowings = _borrowingRepository.GetAllAsync(cancellationToken)
            .Result
            .Where(x => x.Status == BorrowingStatus.Active && x.DueDate < DateTime.UtcNow);
        foreach(var borrowing in borrowings)
        {
            borrowing.Status = BorrowingStatus.Overdue;
            await _borrowingRepository.UpdateAsync(borrowing);
        }
    }
}
