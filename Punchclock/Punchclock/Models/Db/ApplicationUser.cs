namespace Punchclock.Models.Db;

public class ApplicationUser
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public List<Entry> Entries { get; set; } = null!;
}