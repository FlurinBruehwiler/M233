namespace Punchclock.Models;

public class Tag
{
    public long Id { get; set; }
    public string Title { get; set; }
    public List<Entry> Entries { get; set; }
}