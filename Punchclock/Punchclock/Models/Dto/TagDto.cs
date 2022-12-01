namespace Punchclock.Models.Dto;

public class TagDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public List<long> Entries { get; set; } = new();
}