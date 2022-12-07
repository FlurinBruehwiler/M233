namespace Projektarbeit.Endpoints.UserEndpoints.Dtos;

public class UserResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<int> Bookings { get; set; } = new();
}