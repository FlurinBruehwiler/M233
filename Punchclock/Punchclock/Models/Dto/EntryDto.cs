namespace Punchclock.Models.Dto;

public class EntryDto
{
    public long Id { get; set; }
    public required DateTime CheckIn { get; set; }
    public required DateTime CheckOut { get; set; }
    public long Category { get; set; }
    public List<long> Entries { get; set; }
}