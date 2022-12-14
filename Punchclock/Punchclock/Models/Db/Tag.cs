namespace Punchclock.Models.Db;

public class Tag
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public List<Entry> Entries { get; set; } = null!;
}