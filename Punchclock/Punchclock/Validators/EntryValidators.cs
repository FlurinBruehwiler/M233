using FluentValidation;
using Punchclock.Models;

namespace Punchclock.Validators;

public class EntryValidators : AbstractValidator<EntryDto>
{
    public EntryValidators()
    {
        RuleFor(x => x.CheckOut)
            .NotNull()
            .Must((entry, checkOut) => checkOut < entry.CheckIn)
            .WithMessage("CheckOut must be after CheckIn");

        RuleFor(x => x.CheckIn)
            .NotNull();

        RuleFor(x => x.Category)
            .NotNull();
    }
}