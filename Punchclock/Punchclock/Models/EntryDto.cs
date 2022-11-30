namespace Punchclock.Models;

public class EntryDto
{
    public long Id { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}