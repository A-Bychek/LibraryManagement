using FluentValidation;
using LibraryManagement.Contract.QueryModels.Borrowings;

namespace LibraryManagement.Application.Validation.Borrowings;

public class BorrowingSearchArgsValidator : AbstractValidator<BorrowingSearchArgs>
{
    public BorrowingSearchArgsValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(1).WithMessage("AuthorId should be positive.").WithErrorCode("422");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status should be defined as Active (0), Returned (1) or Overdue (2).").WithErrorCode("422");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber should be positive.").WithErrorCode("422")
            .LessThanOrEqualTo(50).WithMessage("PageSize cannot be more than 50.").WithErrorCode("422");
    }
}
