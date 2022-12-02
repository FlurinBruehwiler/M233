using Microsoft.AspNetCore.Mvc;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Services;
using Punchclock.Validators.ValidationFramework;

namespace Punchclock.Endpoints;

public class AuthenticationEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost( "/register", Register)
            .AddEndpointFilter<ValidatorFilter<UserDto>>()
            .WithOpenApi();
        
        app.MapPost( "/login", Login)
            .WithOpenApi();
    }

    private async Task<IResult> Register(UserDto userDtoAuth, [FromServices] UserService userService, PunchclockDbContext punchclockDbContext)
    {
        await userService.RegisterUser(userDtoAuth);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }
    
    private async Task<IResult> Login(UserDto userDto, [FromServices] UserService userService)
    {
        await userService.LoginUser(userDto);
        return Results.Ok();
    }
}