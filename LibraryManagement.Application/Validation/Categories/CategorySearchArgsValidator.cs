using FluentValidation;
using LibraryManagement.Contract.QueryModels.Categories;

namespace LibraryManagement.Application.Validation.Categories;

public class CategorySearchArgsValidator : AbstractValidator<CategorySearchArgs>
{
    public CategorySearchArgsValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MinimumLength(3).WithMessage("Search term must be at least 3 characters long.").WithErrorCode("422")
            .MaximumLength(200).WithMessage("Search term cannot be more than 200 characters.").WithErrorCode("422")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));


        RuleFor(x => x.ParentCategoryId)
            .GreaterThanOrEqualTo(1).WithMessage("ParentCategoryId should be positive.").WithErrorCode("422");
    }
}
