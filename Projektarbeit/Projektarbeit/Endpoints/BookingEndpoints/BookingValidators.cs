using FluentValidation;
using Projektarbeit.Endpoints.BookingEndpoints.Dtos;

namespace Projektarbeit.Endpoints.BookingEndpoints;

public class PatchBookingRequestDtoValidator : AbstractValidator<PatchBookingRequestDto>
{
    public PatchBookingRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}