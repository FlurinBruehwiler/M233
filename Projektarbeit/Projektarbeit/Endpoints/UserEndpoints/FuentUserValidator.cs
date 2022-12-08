using FluentValidation;
using Projektarbeit.Models;

namespace Projektarbeit.Endpoints.UserEndpoints;

public class FluentUserValidator : AbstractValidator<User>
{
    public FluentUserValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}