using FluentValidation;
using Punchclock.Models.Dto;

namespace Punchclock.Validators;

public class CategoryValidators : AbstractValidator<CategoryDto>
{
    public CategoryValidators()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .MaximumLength(30);
    }
}