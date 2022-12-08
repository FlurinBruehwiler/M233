using Projektarbeit.Errors;
using Projektarbeit.Models;

namespace Projektarbeit.Validators;

public class BookingValidator : IValidator<Booking>
{
    public IEnumerable<Error> Validate(Booking obj, List<string> changedProperties, User actingUser)
    {
        if (actingUser.IsAdministrator)
        {
            
        }
        else
        {
            if (changedProperties.Contains(nameof(obj.Status)) && obj.Status == Status.Angenommen)
            {
                yield return new Error("CannotApproveOwnStatus",
                    "Wenn sie nicht Administrator sind, können sie nicht ihre eigenes Booking annehmen");
            }
        }

        if (changedProperties.Contains(nameof(obj.Date)) && obj.Date.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
        {
            yield return new Error("CannotChangeDateToPast", "Cannot set Booking Date into past");
        }
    }
}