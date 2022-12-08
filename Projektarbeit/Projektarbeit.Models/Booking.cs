namespace Projektarbeit.Models;

public class Booking
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public Time Time { get; set; }
    public Status Status { get; set; }
    public int ParticipationCount { get; set; }
    public required User User { get; set; }
    public int UserId { get; set; }
}

public enum Time
{
    Vormittag, 
    Nachmittag, 
    Ganztagig
}

public enum Status
{
    Beantragt,
    Angenommen,
    Abgelehnt
}