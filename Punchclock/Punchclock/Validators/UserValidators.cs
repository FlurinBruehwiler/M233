using FluentValidation;
using Punchclock.Models.Dto;

namespace Punchclock.Validators;

public class UserValidators : AbstractValidator<UserDto>
{
    public UserValidators()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The username cannot be empty");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("The password cannot be empty");
        
        RuleFor(x => x.Password)
            .MinimumLength(6)
            .WithMessage("The password has to be at least six characters long");
    }
}