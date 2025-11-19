using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;

namespace LibraryManagement.Application.Validation.Borrowings
{
    public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
    {
        public ReturnBookCommandValidator()
        {
            RuleFor(x => x.BorrowingId)
                .NotNull().WithMessage("Book isn't returned. Borrowing ID cannot be null.").WithErrorCode("422")
                .NotEmpty().WithMessage("Book isn't returned. Borrowing ID cannot be empty.").WithErrorCode("422");
        }
    }
}
