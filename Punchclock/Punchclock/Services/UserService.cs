using Microsoft.EntityFrameworkCore;
using Punchclock.Models;
using Punchclock.Models.Dto;

namespace Punchclock.Services;

public class UserService
{
    private readonly AuthService _authService;
    private readonly PunchclockDbContext _punchclockDbContext;

    public UserService(AuthService authService, PunchclockDbContext punchclockDbContext)
    {
        _authService = authService;
        _punchclockDbContext = punchclockDbContext;
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
    
    public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
    {
        var user = await _punchclockDbContext.Users.FirstOrDefaultAsync(x => x.Name == username);
        if(user is null)
            throw new BadRequestException(Errors.UserNotFound);
        return user;
    }
}