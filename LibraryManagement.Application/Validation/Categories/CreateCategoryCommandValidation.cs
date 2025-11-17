using FluentValidation;
using LibraryManagement.Application.Commands.Category;

namespace LibraryManagement.Application.Validation.Categories
{
    public class CreateCategoryCommandValidation: AbstractValidator<CreateCategoryCommand>
    {
        CreateCategoryCommandValidation() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category isn't created. Name cannot be empty.").WithErrorCode("422")
                .NotNull().WithMessage("Category isn't created. Name cannot be null.").WithErrorCode("422")
                .MaximumLength(200).WithMessage("Category isn't created. Name cannot be more than 200 characters.").WithErrorCode("422");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Category isn't created. Description cannot be more than 2000 characters.").WithErrorCode("422");
        }
    }
}
