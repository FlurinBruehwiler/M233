namespace Punchclock.Models.Dto;

public class CategoryDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public List<long> Entries { get; set; }
}