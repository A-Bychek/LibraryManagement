using FluentValidation;
using LibraryManagement.Application.Commands.Book;
using System.Globalization;

namespace LibraryManagement.Application.Validation.Books
{
    public class UpdateBookCommandValidation: AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidation() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Book entity didn't updated. Title cannot be empty.").WithErrorCode("422")
                .MinimumLength(3).WithMessage("Book entity didn't updated. Title must be at least 3 characters long.").WithErrorCode("422")
                .MaximumLength(200).WithMessage("Book entity didn't updated. Title cannot be more than 200 characters.").WithErrorCode("422");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Book entity didn't updated. Description cannot be more than 2000 characters.").WithErrorCode("422");

            RuleFor(x => x.PublishedDate)
                .Must((publishedDate) =>
                DateTime.TryParseExact(
                    publishedDate,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _)).WithMessage("Book entity didn't updated. Published date should be passed in the Year-Month-Day format.").WithErrorCode("422");

            RuleFor(x => x.PageCount)
                .LessThanOrEqualTo(1000).WithMessage("Book entity didn't updated. Page count cannot be more than 1000.").WithErrorCode("422");
        }
    }
}
