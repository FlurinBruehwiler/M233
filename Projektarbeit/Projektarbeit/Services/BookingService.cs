using Microsoft.EntityFrameworkCore;
using Projektarbeit.Errors;
using Projektarbeit.Extensions;
using Projektarbeit.Models;

namespace Projektarbeit.Services;

public class BookingService
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserService _userService;
    private readonly SaveService _saveService;

    public BookingService(DatabaseContext databaseContext, UserService userService, SaveService saveService)
    {
        _databaseContext = databaseContext;
        _userService = userService;
        _saveService = saveService;
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        return await _databaseContext.Bookings
            .Include(x => x.User)
            .ToListAsync();
    }
    
    public async Task<List<Booking>> GetOwnBooks(User user)
    {
        return await _databaseContext.Bookings
            .Where(x => x.User == user)
            .Include(x => x.User)
            .ToListAsync();
    }

    public async Task<Booking> CreateBooking(CreateBookingRequestDto bookingToCreate)
    {
        var booking = new Booking
        {
            Date = bookingToCreate.Date,
            Status = Status.Beantragt,
            Time = bookingToCreate.Time,
            User = await _userService.GetUser(),
            ParticipationCount = bookingToCreate.ParticipationCount
        };
        _databaseContext.Bookings.Add(booking);
        return booking;
    }

    public async Task DeleteBooking(int id)
    {
        var bookingToDelete = await _databaseContext.Users.FirstOrNotFoundAsync(x => x.Id == id);
        
        _databaseContext.Users.Remove(bookingToDelete);
    }

    public async Task PatchBooking(PatchBookingRequestDto patchUserRequestDto)
    {
        var currentUser = await _userService.GetUser();
        
        var bookingToPatch = await _databaseContext.Bookings
            .FirstOrNotFoundAsync(x => x.Id == patchUserRequestDto.Id);

        if (!currentUser.IsAdministrator && bookingToPatch.UserId != currentUser.Id)
            throw new BadRequestException(new Error("Error", "Cannot modify someone elses booking"));
        
        if (bookingToPatch.Date < DateOnly.FromDateTime(DateTime.Now) || patchUserRequestDto.Date < DateOnly.FromDateTime(DateTime.Now))
            throw new BadRequestException(new Error("CannotModifyBookingInPast", "A user cant modify a booking in the past"));
        
        if(!currentUser.IsAdministrator && bookingToPatch.Status is not Status.Beantragt)
            throw new BadRequestException(new Error("Error", "Cannot change booking which is already approved or denied"));
        
        if (patchUserRequestDto.Date is not null)
            bookingToPatch.Date = patchUserRequestDto.Date.Value;
        
        if (patchUserRequestDto.Time is not null)
            bookingToPatch.Time = patchUserRequestDto.Time.Value;
        
        if (patchUserRequestDto.ParticipationCount is not null)
            bookingToPatch.ParticipationCount = patchUserRequestDto.ParticipationCount.Value;

        if (patchUserRequestDto.Status is not null && currentUser.IsAdministrator)
            bookingToPatch.Status = patchUserRequestDto.Status.Value;

        if (patchUserRequestDto.Status is Status.Abgelehnt)
            bookingToPatch.Status = patchUserRequestDto.Status.Value;

        if (patchUserRequestDto.User is not null && currentUser.IsAdministrator)
        {
            var user = await _databaseContext.Users
                .FirstOrNotFoundAsync(x => x.Id == patchUserRequestDto.User);

            bookingToPatch.User = user;
        }
    }
}