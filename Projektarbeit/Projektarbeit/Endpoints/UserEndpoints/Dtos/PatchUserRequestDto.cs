namespace Projektarbeit.Endpoints.UserEndpoints.Dtos;

public class PatchUserRequestDto
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public List<int>? Bookings { get; set; }
    public bool? IsAdministrator { get; set; }
}