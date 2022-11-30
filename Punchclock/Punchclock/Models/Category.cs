namespace Punchclock.Models;

public class Category
{
    public long Id { get; set; }
    public string Title { get; set; }
    public List<Entry> Entries { get; set; }
}