using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Projektarbeit.Endpoints.AuthenticationEndpoints;
using Projektarbeit.Endpoints.UserEndpoints.Dtos;
using Projektarbeit.Errors;
using Projektarbeit.Models;

namespace Projektarbeit.Services;

public class UserService
{
    private readonly DatabaseContext _databaseContext;
    private readonly AuthService _authService;
    private readonly HttpContextAccessor _contextAccessor;

    public UserService(DatabaseContext databaseContext, AuthService authService, HttpContextAccessor contextAccessor)
    {
        _databaseContext = databaseContext;
        _authService = authService;
        _contextAccessor = contextAccessor;
    }
    
    public async Task RegisterUser(UserDto userDto)
    {
        if (await _databaseContext.Users.AnyAsync(x => x.Name == userDto.Name))
            throw new BadRequestException(Errors.Errors.UsernameAlreadyExists);
        
        _authService.CreatePasswordHash(userDto.Password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Name = userDto.Name,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = userDto.Email
        };
        _databaseContext.Users.Add(user);
        _authService.AppendAccessToken(user);
    }

    public async Task LoginUser(UserDto userDto)
    {
        var user = await GetUserByUsernameAsync(userDto.Name);

        if (!_authService.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException(Errors.Errors.WrongPassword);
    
        _authService.AppendAccessToken(user);
    }

    private async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Name == username);
        if(user is null)
            throw new BadRequestException(Errors.Errors.UserNotFound);
        return user;
    }

    private string? GetUsername()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<User> GetUser()
    {
        var username = GetUsername();
        if (username is null)
            throw new BadRequestException(Errors.Errors.NoAuth);

        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Name == username);
        if (user is null)
            throw new BadRequestException(Errors.Errors.UserNotFound);

        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _databaseContext.Users
            .Include(x => x.Bookings)
            .ToListAsync();
    }

    public async Task<User> CreateUser(CreateUserRequestDto createUserTo)
    {
        _authService.CreatePasswordHash(createUserTo.Password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Email = createUserTo.Email,
            Name = createUserTo.Name,
            IsAdministrator = createUserTo.IsAdministrator,
            Bookings = new List<Booking>(),
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        foreach (var bookingId in createUserTo.Bookings)
        {
            var booking = await _databaseContext.Bookings
                .FirstOrDefaultAsync(x => x.Id == bookingId);

            if (booking is null)
                throw new BadRequestException(Errors.Errors.BookingNotFound);
            
            user.Bookings.Add(booking);
        }
        _databaseContext.Users.Add(user);
        await _databaseContext.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUser(int id)
    {
        var userToDelete = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (userToDelete is null)
            throw new BadRequestException(Errors.Errors.UserNotFound);
        
        _databaseContext.Users.Remove(userToDelete);
    }

    public async Task PatchUser(PatchUserRequestDto patchUserRequestDto)
    {
        var userToPatch = await _databaseContext.Users
            .FirstOrDefaultAsync(x => x.Id == patchUserRequestDto.Id);

        if (userToPatch is null)
            throw new BadRequestException(Errors.Errors.UserNotFound);

        if (patchUserRequestDto.Name is not null)
            userToPatch.Name = patchUserRequestDto.Name;
        
        if (patchUserRequestDto.Email is not null)
            userToPatch.Email = patchUserRequestDto.Email;
        
        if (patchUserRequestDto.IsAdministrator is not null)
            userToPatch.IsAdministrator = patchUserRequestDto.IsAdministrator.Value;

        if (patchUserRequestDto.Password is not null)
        {
            _authService.CreatePasswordHash(patchUserRequestDto.Password, out var passwordHash, out var passwordSalt);
            userToPatch.PasswordHash = passwordHash;
            userToPatch.PasswordSalt = passwordSalt;
        }

        if (patchUserRequestDto.Bookings is not null)
        {
            foreach (var bookingId in patchUserRequestDto.Bookings)
            {
                var booking = await _databaseContext.Bookings
                    .FirstOrDefaultAsync(x => x.Id == bookingId);

                if (booking is null)
                    throw new BadRequestException(Errors.Errors.BookingNotFound);
            
                userToPatch.Bookings.Add(booking);
            }
        }
    }
}