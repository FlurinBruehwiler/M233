using Projektarbeit.Models;

namespace Projektarbeit.Endpoints.BookingEndpoints.Dtos;

public class CreateBookingRequestDto
{
    public DateOnly Date { get; set; }
    public Time Time { get; set; }
    public int ParticipationCount { get; set; }
    public int? User { get; set; }
}