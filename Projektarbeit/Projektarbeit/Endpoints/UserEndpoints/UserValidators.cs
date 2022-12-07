using FluentValidation;
using Projektarbeit.Endpoints.UserEndpoints.Dtos;

namespace Projektarbeit.Endpoints.UserEndpoints;

public class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator()
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

public class PatchUserRequestDtoValidator : AbstractValidator<PatchUserRequestDto>
{
    public PatchUserRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}