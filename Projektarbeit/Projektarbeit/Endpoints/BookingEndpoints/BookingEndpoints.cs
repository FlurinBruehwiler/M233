using Projektarbeit.Endpoints.BookingEndpoints.Dtos;
using Projektarbeit.Errors;
using Projektarbeit.Mappers;
using Projektarbeit.Services;

namespace Projektarbeit.Endpoints.BookingEndpoints;

public class BookingEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/bookings", GetBookings)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapPost("/bookings", CreateBooking)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapPatch("/bookings", PatchBooking)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapDelete("/bookings/{id:int}", DeleteBooking)
            .RequireAuthorization()
            .WithOpenApi();
    }

    private async Task<IResult> DeleteBooking(int id, BookingService bookingService, SaveService saveService)
    {
        await bookingService.DeleteBooking(id);
        await saveService.SaveChangesAndValidateAsync();
        return Results.Ok();
    }

    private async Task<IResult> PatchBooking(PatchBookingRequestDto bookingToPatch, BookingService bookingService, SaveService saveService)
    {
        await bookingService.PatchBooking(bookingToPatch);
        await saveService.SaveChangesAndValidateAsync();
        return Results.Ok();
    }

    private async Task<IResult> CreateBooking(CreateBookingRequestDto bookingToCreate, UserService userService, BookingService bookingService, SaveService saveService)
    {
        var user = await userService.GetUser();

        if (!user.IsAdministrator && bookingToCreate.User is not null)
            throw new BadRequestException(Errors.Errors.BookingOnlyForYourself);
        
        bookingToCreate.User ??= user.Id;

        var createdBooking = await bookingService.CreateBooking(bookingToCreate);

        await saveService.SaveChangesAndValidateAsync();
        
        var createdBookingDto = createdBooking.ToResponseDto();
        
        return Results.Created("/bookings", createdBookingDto);
    }

    private async Task<IResult> GetBookings(UserService userService, BookingService bookingService)
    {
        var user = await userService.GetUser();
        var bookings = user.IsAdministrator
            ? await bookingService.GetAllBookings()
            : await bookingService.GetOwnBooks(user);
        
        var dtoBookings = bookings
            .Select(x => x.ToResponseDto())
            .ToList();
        
        return Results.Ok(dtoBookings);
    }
}