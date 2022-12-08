namespace Projektarbeit.Models;

public class RegisterRequestDto
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}