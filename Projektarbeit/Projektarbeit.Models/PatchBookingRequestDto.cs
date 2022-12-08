namespace Projektarbeit.Models;

public class PatchBookingRequestDto
{
    public required int Id { get; set; }
    public DateOnly? Date { get; set; }
    public Time? Time { get; set; }
    public Status? Status { get; set; }
    public int? ParticipationCount { get; set; }
    public int? User { get; set; }
}