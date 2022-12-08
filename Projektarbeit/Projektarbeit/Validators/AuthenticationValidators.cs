using FluentValidation;
using Projektarbeit.Models;

namespace Projektarbeit.Validators;

public class RequestUserDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RequestUserDtoValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);
        
        RuleFor(x => x.Lastname)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);
    }
}