using FluentValidation;
using Punchclock.Models.Dto;

namespace Punchclock.Validators;

public class TagValidators : AbstractValidator<TagDto>
{
    public TagValidators()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .MaximumLength(30);
    }
}