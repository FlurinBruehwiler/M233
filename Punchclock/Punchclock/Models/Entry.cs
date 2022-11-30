namespace Punchclock.Models;

public class Entry
{
    public long Id { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public Category Category { get; set; }
    public long CategoryId { get; set; }
    public List<Tag> Tags { get; set; }
}