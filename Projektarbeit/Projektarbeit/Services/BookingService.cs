using Microsoft.EntityFrameworkCore;
using Projektarbeit.Endpoints.BookingEndpoints;
using Projektarbeit.Errors;
using Projektarbeit.Models;

namespace Projektarbeit.Services;

public class BookingService
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserService _userService;

    public BookingService(DatabaseContext databaseContext, UserService userService)
    {
        _databaseContext = databaseContext;
        _userService = userService;
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
        await _databaseContext.SaveChangesAsync();
        return booking;
    }

    public async Task DeleteBooking(int id)
    {
        var bookingToDelete = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (bookingToDelete is null)
            throw new BadRequestException(Errors.Errors.UserNotFound);

        _databaseContext.Users.Remove(bookingToDelete);
    }

    public async Task PatchBooking(PatchBookingRequestDto patchUserRequestDto)
    {
        var currentUser = await _userService.GetUser();
        
        var bookingToPatch = await _databaseContext.Bookings
            .FirstOrDefaultAsync(x => x.Id == patchUserRequestDto.Id);

        if (bookingToPatch is null)
            throw new BadRequestException(Errors.Errors.UserNotFound);

        if (patchUserRequestDto.Date is not null)
            bookingToPatch.Date = patchUserRequestDto.Date.Value;
        
        if (patchUserRequestDto.Time is not null)
            bookingToPatch.Time = patchUserRequestDto.Time.Value;
        
        if (patchUserRequestDto.ParticipationCount is not null)
            bookingToPatch.ParticipationCount = patchUserRequestDto.ParticipationCount.Value;

        if (patchUserRequestDto.Status is not null && currentUser.IsAdministrator)
            bookingToPatch.Status = patchUserRequestDto.Status.Value;

        if (patchUserRequestDto.User is not null && currentUser.IsAdministrator)
        {
            var user = await _databaseContext.Users
                .FirstOrDefaultAsync(x => x.Id == patchUserRequestDto.User);

            if (user is null)
                throw new BadRequestException(Errors.Errors.UserNotFound);

            bookingToPatch.User = user;
        }
    }
}