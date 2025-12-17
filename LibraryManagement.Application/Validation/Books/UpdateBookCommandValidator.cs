using FluentValidation;
using LibraryManagement.Contract.Commands.Book;
using System.Globalization;

namespace LibraryManagement.Application.Validation.Books;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {   RuleFor(x => x.BookId)
            .GreaterThan(0).WithMessage("Book entity didn't updated. Pass the valid book ID.").WithErrorCode("422");

        RuleFor(x => x.Title)
            .MinimumLength(3).WithMessage("Book entity didn't updated. Title must be at least 3 characters long.").WithErrorCode("422")
            .MaximumLength(200).WithMessage("Book entity didn't updated. Title cannot be more than 200 characters.").WithErrorCode("422")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Book entity didn't updated. Description cannot be more than 2000 characters.").WithErrorCode("422");

        RuleFor(x => x.PublishedDate)
            .Must((publishedDate) =>
            DateTime.TryParseExact(
                publishedDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _)).WithMessage("Book entity didn't updated. Published date should be passed in the Year-Month-Day format.").WithErrorCode("422")
            .When(x => !string.IsNullOrEmpty(x.PublishedDate));

        RuleFor(x => x.PageCount)
            .LessThanOrEqualTo(1000).WithMessage("Book entity didn't updated. Page count cannot be more than 1000.").WithErrorCode("422");
    }
}
