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
    
    public async Task RegisterUser(DtoAuthUser dtoAuthUser, HttpResponse httpResponse)
    {
        if(string.IsNullOrWhiteSpace(dtoAuthUser.Name))
            throw new BadRequestException(Errors.UsernameEmpty);
            
        if(string.IsNullOrWhiteSpace(dtoAuthUser.Password))
            throw new BadRequestException(Errors.PasswordEmpty);
        
        if (await _punchclockDbContext.Users.AnyAsync(x => x.Name == dtoAuthUser.Name))
            throw new BadRequestException(Errors.UsernameAlreadyExists);
        
        _authService.CreatePasswordHash(dtoAuthUser.Password, out var passwordHash, out var passwordSalt);
        var user = new User
        {
            Name = dtoAuthUser.Name,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        _punchclockDbContext.Users.Add(user);
        await _punchclockDbContext.SaveChangesAsync();
        _authService.AppendAccessToken(httpResponse, user);
    }

    public async Task LoginUser(DtoAuthUser dtoAuthUser, HttpResponse httpResponse)
    {
        var user = await GetUserByUsernameAsync(dtoAuthUser.Name);

        if (!_authService.VerifyPasswordHash(dtoAuthUser.Password, user.PasswordHash, user.PasswordSalt))
            throw new BadRequestException(Errors.WrongPassword);
    
        _authService.AppendAccessToken(httpResponse, user);
    }
    
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _punchclockDbContext.Users.FirstOrDefaultAsync(x => x.Name == username);
        if(user is null)
            throw new BadRequestException(Errors.UserNotFound);
        return user;
    }
}