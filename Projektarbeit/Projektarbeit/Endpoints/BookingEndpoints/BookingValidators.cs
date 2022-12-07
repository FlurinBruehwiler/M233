using FluentValidation;
using Projektarbeit.Endpoints.BookingEndpoints.Dtos;

namespace Projektarbeit.Endpoints.BookingEndpoints;

public class CreateBookingRequestDtoValidator : AbstractValidator<CreateBookingRequestDto>
{
    public CreateBookingRequestDtoValidator()
    {
        RuleFor(x => x.ParticipationCount)
            .NotEmpty()
            .Must(i => i > 0);

        RuleFor(x => x.Date)
            .NotEmpty()
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now));
    }
}

public class PatchBookingRequestDtoValidator : AbstractValidator<PatchBookingRequestDto>
{
    public PatchBookingRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}