namespace Punchclock.Models;

public class TagDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public List<long> Entries { get; set; }
}