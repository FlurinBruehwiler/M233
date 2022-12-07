namespace Projektarbeit.Endpoints.UserEndpoints.Dtos;

public class CreateUserRequestDto
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public List<int> Bookings { get; set; } = new();
    public bool IsAdministrator { get; set; }
}

