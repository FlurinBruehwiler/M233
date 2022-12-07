using Projektarbeit.Endpoints.BookingEndpoints;
using Projektarbeit.Endpoints.UserEndpoints.Dtos;
using Projektarbeit.Models;

namespace Projektarbeit.Mappers;

public static class Mappers
{
    public static UserResponseDto ToResponseDto(this User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Bookings = user.Bookings.Select(x => x.Id).ToList()
        };
    }
    
    public static BookingResponseDto ToResponseDto(this Booking booking)
    {
        return new BookingResponseDto
        {

        };
    }
}