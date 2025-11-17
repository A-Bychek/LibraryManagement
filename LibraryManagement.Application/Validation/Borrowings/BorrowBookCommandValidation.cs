using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;

namespace LibraryManagement.Application.Validation.Borrowings
{
    public class BorrowBookCommandValidation: AbstractValidator<BorrowBookCommand>
    {
        public BorrowBookCommandValidation() 
        {
            RuleFor(x => x.BookId)
                .NotNull().WithMessage("Book isn't borrowed. Book ID cannot be null.").WithErrorCode("422")
                .NotEmpty().WithMessage("Book isn't borrowed. Book ID cannot be empty.").WithErrorCode("422");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("Book isn't borrowed. User ID cannot be null.").WithErrorCode("422")
                .NotEmpty().WithMessage("Book isn't borrowed. User ID cannot be empty.").WithErrorCode("422");

            RuleFor(x => x.DaysToReturn)
                .LessThanOrEqualTo(14).WithMessage("Book isn't borrowed. Days to return should be less or equal to 14.").WithErrorCode("422");
        }
    }
}
