namespace Projektarbeit.Models;

public class CreateBookingRequestDto
{
    public DateOnly Date { get; set; }
    public Time Time { get; set; }
    public int ParticipationCount { get; set; }
    public int? User { get; set; }
}