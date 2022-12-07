namespace Projektarbeit.Endpoints.UserEndpoints.Dtos;

public class CreateUserRequestDto
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public List<int> Bookings { get; set; } = new();
    public bool IsAdministrator { get; set; }
}

