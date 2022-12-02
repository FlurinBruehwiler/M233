using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Punchclock.Models;
using Punchclock.Models.Dto;

namespace Punchclock.Services;

public class UserService
{
    private readonly AuthService _authService;
    private readonly PunchclockDbContext _punchclockDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(AuthService authService, PunchclockDbContext punchclockDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _punchclockDbContext = punchclockDbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task RegisterUser(UserDto userDto)
    {
        if(string.IsNullOrWhiteSpace(userDto.Name))
            throw new BadRequestException(Errors.UsernameEmpty);
            
        if(string.IsNullOrWhiteSpace(userDto.Password))
            throw new BadRequestException(Errors.PasswordEmpty);
        
        if (await _punchclockDbContext.Users.AnyAsync(x => x.Name == userDto.Name))
            throw new BadRequestException(Errors.UsernameAlreadyExists);
        
        _authService.CreatePasswordHash(userDto.Password, out var passwordHash, out var passwordSalt);
        var user = new ApplicationUser
        {
            Name = userDto.Name,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        _punchclockDbContext.Users.Add(user);
        await _punchclockDbContext.SaveChangesAsync();
        _authService.AppendAccessToken(user);
    }

    public async Task LoginUser(UserDto userDto)
    {
        var user = await GetUserByUsernameAsync(userDto.Name);

        if (!_authService.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException(Errors.WrongPassword);
    
        _authService.AppendAccessToken(user);
    }

    private async Task<ApplicationUser> GetUserByUsernameAsync(string username)
    {
        var user = await _punchclockDbContext.Users.FirstOrDefaultAsync(x => x.Name == username);
        if(user is null)
            throw new BadRequestException(Errors.UserNotFound);
        return user;
    }

    private string? GetUsername()
    {
        if (_httpContextAccessor.HttpContext == null)
            return null;
            
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public ApplicationUser GetUser()
    {
        var username = GetUsername();
        if (username is null)
            throw new BadRequestException(Errors.NoAuth);
        return _punchclockDbContext.Users.FirstOrDefault(x => x.Name == username) ?? throw new Exception();
    }

    public ApplicationUser? GetUserById(int id)
    {
        return _punchclockDbContext.Users.FirstOrDefault(x => x.Id == id);
    }
}