using FluentValidation;
using LibraryManagement.Contract.QueryModels.Books;

namespace LibraryManagement.Application.Validation.Books;

public class BookSearchArgsValidator : AbstractValidator<BookSearchArgs>
{
    public BookSearchArgsValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MinimumLength(3).WithMessage("Search term must be at least 3 characters long.").WithErrorCode("422")
            .MaximumLength(200).WithMessage("Search term cannot be more than 200 characters.").WithErrorCode("422");

        RuleFor(x => x.AuthorId)
            .GreaterThanOrEqualTo(1).WithMessage("AuthorId should be positive.").WithErrorCode("422");

        RuleFor(x => x.CategoryId)
            .GreaterThanOrEqualTo(1).WithMessage("CategoryId should be positive.").WithErrorCode("422");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber should be positive.").WithErrorCode("422");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber should be positive.").WithErrorCode("422")
            .LessThanOrEqualTo(50).WithMessage("PageSize cannot be more than 50.").WithErrorCode("422");
    }
}
