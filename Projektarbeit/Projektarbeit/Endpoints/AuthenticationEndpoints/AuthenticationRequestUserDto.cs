namespace Projektarbeit.Endpoints.AuthenticationEndpoints;

public class UserDto
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}