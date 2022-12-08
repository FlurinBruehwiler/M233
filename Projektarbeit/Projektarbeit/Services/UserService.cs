using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Projektarbeit.Errors;
using Projektarbeit.Extensions;
using Projektarbeit.Models;

namespace Projektarbeit.Services;

public class UserService
{
    private readonly DatabaseContext _databaseContext;
    private readonly AuthService _authService;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(DatabaseContext databaseContext,
        AuthService authService,
        IHttpContextAccessor contextAccessor)
    {
        _databaseContext = databaseContext;
        _authService = authService;
        _contextAccessor = contextAccessor;
    }

    public async Task RegisterUser(RegisterRequestDto registerRequestDto)
    {
        if (await _databaseContext.Users.AnyAsync(x => x.Email == registerRequestDto.Email))
            throw new BadRequestException(Errors.Errors.EmailAlreadyExists);

        AuthService.CreatePasswordHash(registerRequestDto.Password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Email = registerRequestDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Firstname = registerRequestDto.Firstname,
            LastName = registerRequestDto.Lastname,
            IsAdministrator = false
        };
        var count = await _databaseContext.Users.CountAsync();
        if (count == 0)
            user.IsAdministrator = true;
        _databaseContext.Users.Add(user);
        _authService.AppendAccessToken(user);
    }

    public async Task LoginUser(LoginRequestDto loginRequestDto)
    {
        var user = await GetUserByEmailAsync(loginRequestDto.Email);

        if (!_authService.VerifyPasswordHash(loginRequestDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException(Errors.Errors.WrongPassword);

        _authService.AppendAccessToken(user);
    }

    private async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _databaseContext.Users.FirstOrNotFoundAsync(x => x.Email == email);

        return user;
    }

    private string? GetEmail()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<User> GetUser()
    {
        var email = GetEmail();
        if (email is null)
            throw new BadRequestException(Errors.Errors.NoAuth);

        var user = await _databaseContext.Users.FirstOrNotFoundAsync(x => x.Email == email);

        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _databaseContext.Users
            .Include(x => x.Bookings)
            .ToListAsync();
    }

    public async Task<User> CreateUser(CreateUserRequestDto userToCreate)
    {
        AuthService.CreatePasswordHash(userToCreate.Password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Email = userToCreate.Email,
            Firstname = userToCreate.Firstname,
            LastName = userToCreate.Lastname,
            IsAdministrator = userToCreate.IsAdministrator,
            Bookings = new List<Booking>(),
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        foreach (var bookingId in userToCreate.Bookings)
        {
            var booking = await _databaseContext.Bookings
                .FirstOrNotFoundAsync(x => x.Id == bookingId);

            user.Bookings.Add(booking);
        }

        _databaseContext.Users.Add(user);
        return user;
    }

    public async Task DeleteUser(int id)
    {
        var userToDelete = await _databaseContext.Users.FirstOrNotFoundAsync(x => x.Id == id);

        _databaseContext.Users.Remove(userToDelete);
    }

    public async Task PatchUser(PatchUserRequestDto patchUserRequestDto)
    {
        var userToPatch = await _databaseContext.Users
            .FirstOrNotFoundAsync(x => x.Id == patchUserRequestDto.Id);

        if (patchUserRequestDto.Email is not null)
            userToPatch.Email = patchUserRequestDto.Email;

        if (patchUserRequestDto.Firstname is not null)
            userToPatch.Firstname = patchUserRequestDto.Firstname;

        if (patchUserRequestDto.Lastname is not null)
            userToPatch.LastName = patchUserRequestDto.Lastname;

        if (patchUserRequestDto.IsAdministrator is not null)
            userToPatch.IsAdministrator = patchUserRequestDto.IsAdministrator.Value;

        if (patchUserRequestDto.Password is not null)
        {
            AuthService.CreatePasswordHash(patchUserRequestDto.Password, out var passwordHash, out var passwordSalt);
            userToPatch.PasswordHash = passwordHash;
            userToPatch.PasswordSalt = passwordSalt;
        }

        if (patchUserRequestDto.Bookings is not null)
        {
            foreach (var bookingId in patchUserRequestDto.Bookings)
            {
                var booking = await _databaseContext.Bookings
                    .FirstOrNotFoundAsync(x => x.Id == bookingId);

                userToPatch.Bookings.Add(booking);
            }
        }
    }
}