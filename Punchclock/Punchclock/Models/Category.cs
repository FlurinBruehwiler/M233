namespace Punchclock.Models;

public class Category
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public List<Entry> Entries { get; set; } = null!;
}