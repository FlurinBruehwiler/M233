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
            Firstname = user.Firstname,
            Lastname = user.LastName,
            Bookings = user.Bookings.Select(x => x.Id).ToList()
        };
    }
    
    public static BookingResponseDto ToResponseDto(this Booking booking)
    {
        return new BookingResponseDto
        {
            Id = booking.Id,
            Date = booking.Date,
            Status = booking.Status,
            Time = booking.Time,
            User = booking.UserId,
            ParticipationCount = booking.ParticipationCount
        };
    }
}