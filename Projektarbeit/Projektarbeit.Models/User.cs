namespace Projektarbeit.Models;

public class User
{
    public int Id { get; set; }
    public required string Firstname { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public bool IsAdministrator { get; set; }
    public List<Booking> Bookings { get; set; } = new();
}