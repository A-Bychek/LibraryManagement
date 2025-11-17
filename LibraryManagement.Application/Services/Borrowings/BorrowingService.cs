using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;


namespace LibraryManagement.Application.Services.Borrowings
{
    public class BorrowingService: IBorrowingService
    {
        private const double dailyFine = 1;

        private IBorrowingRepository _borrowingRepository;
        private IBookRepository _bookRepository;
        private IMapper _mapper;

        public BorrowingService(IBorrowingRepository borrowingRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        public async Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand borrowBookCommand, CancellationToken cancellationToken)
        { 
        Book book = await _bookRepository.GetByIdAsync(borrowBookCommand.BookId, cancellationToken);

        if (!book.IsAvailable)
        {
            throw new Exception($"Can't borrow the {book.Title} book");
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
        public async Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand returnBookCommand, CancellationToken cancellationToken)
        {

            var borrowing = await _borrowingRepository.GetByIdAsync(returnBookCommand.BorrowingId, cancellationToken)
                ?? throw new Exception($"Can't find the {returnBookCommand.BorrowingId} boorowing entity!");

            if (borrowing.Status == BorrowingStatus.Active)
            {
                borrowing.Status = BorrowingStatus.Returned;
                borrowing.ReturnDate = DateTime.UtcNow;

                var book = await _bookRepository.GetByIdAsync(borrowing.BookId, cancellationToken)
                       ?? throw new Exception($"Can't find the {borrowing.BookId} book!");
                book.IsAvailable = true;

                await _borrowingRepository.UpdateAsync(borrowing, cancellationToken);
                await _bookRepository.UpdateAsync(book, cancellationToken);
            }

            var fineAmount = await CalculateFineAsync(borrowing.BorrowingId, cancellationToken);
            return _mapper.Map<Borrowing, BorrowingDto>(borrowing, opt => opt.Items["FineAmount"] = fineAmount);
        }
        public async Task<List<BorrowingDto>> GetUserBorrowingsAsync(long userId, CancellationToken cancellationToken)
        {
            var userBorrowings =  _borrowingRepository.GetAllAsync(cancellationToken).Result.Where(b => b.UserId == userId);
            List<BorrowingDto> mappedUserBorrowings = new List<BorrowingDto>();
            foreach (var userBorrowing in userBorrowings)
            {
                mappedUserBorrowings.Append(_mapper.Map<Borrowing, BorrowingDto>(userBorrowing));
            }
            return mappedUserBorrowings;
        }

        public async Task<List<BorrowingDto>> GetOverdueBooksAsync(CancellationToken cancellationToken)
        {
            List<Borrowing> overdueBooks = _borrowingRepository.GetAllAsync(cancellationToken)
                .Result
                .Where(b => b.Status == BorrowingStatus.Active && b.DueDate < DateTime.UtcNow).ToList();
            var mappedBorrowings = new List<BorrowingDto>();
            foreach (var overdueBook in overdueBooks)
            {
                var fineAmount = _mapper.Map<Borrowing, BorrowingDto>(overdueBook);
                mappedBorrowings.Append(_mapper.Map<Borrowing, BorrowingDto>(overdueBook, opt => opt.Items["FineAmount"] = fineAmount));
            }
            return mappedBorrowings;
        }
        public async Task<double> CalculateFineAsync(long borrowingid, CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(borrowingid, cancellationToken);
            if (borrowing.DueDate < DateTime.UtcNow) return dailyFine * (DateTime.UtcNow - borrowing.DueDate).Days;

            return 0;
        }
    }
}
