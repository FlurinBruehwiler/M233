namespace Punchclock.Models.Db;

public class Entry
{
    public long Id { get; set; }
    public required DateTime CheckIn { get; set; }
    public required DateTime CheckOut { get; set; }
    public required Category Category { get; set; }
    public long CategoryId { get; set; }
    public List<Tag> Tags { get; set; } = null!;
    public required ApplicationUser ApplicationUser { get; set; }
    public long ApplicationUserId { get; set; }
}