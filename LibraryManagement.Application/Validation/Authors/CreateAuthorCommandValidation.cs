using LibraryManagement.Application.Commands.Author;
using FluentValidation;
using System.Globalization;

namespace LibraryManagement.Application.Validation.Authors
{
    public class CreateAuthorCommandValidator: AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .MinimumLength(3).WithMessage("Author entity didn't created. First name must be at least 3 characters long.").WithErrorCode("422")
                .MaximumLength(200).WithMessage("Author entity didn't created. First name cannot be more than 200 characters.").WithErrorCode("422")
                .NotEmpty().WithMessage("Author entity didn't created. Fist name cannot be empty.").WithErrorCode("422");
            RuleFor(x => x.LastName)
                .MinimumLength(3).WithMessage("Author entity didn't created. Last name must be at least 3 characters long.").WithErrorCode("422")
                .MaximumLength(200).WithMessage("Author entity didn't created. Last name cannot be more than 200 characters.").WithErrorCode("422");

            RuleFor(x => x.Biography)
                .MaximumLength(2000).WithMessage("Author entity didn't created. Bioghraphy cannot be more than 2000 characters.").WithErrorCode("422");

            RuleFor(x => x.DateOfBirth)
                .Must((dateOfBirth) =>
                DateTime.TryParseExact(
                    dateOfBirth,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _))
                .When(x => !string.IsNullOrEmpty(x.DateOfBirth))
                .WithMessage("Author entity didn't created. Date of birth should be passed in the Year-Month-Day format.").WithErrorCode("422");
        }
    }
}
