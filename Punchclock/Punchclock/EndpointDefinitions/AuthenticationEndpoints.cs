using Microsoft.AspNetCore.Mvc;
using Punchclock.Models.Dto;
using Punchclock.Services;

namespace Punchclock.EndpointDefinitions;

public class AuthenticationEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost( "/register", Register)
            .WithOpenApi();
        
        app.MapPost( "/login", Login)
            .WithOpenApi();
    }

    private async Task<IResult> Register(UserDto userDtoAuth, [FromServices] UserService userService)
    {
        await userService.RegisterUser(userDtoAuth);
        return Results.Ok();
    }
    
    private async Task<IResult> Login(UserDto userDto, [FromServices] UserService userService)
    {
        await userService.LoginUser(userDto);
        return Results.Ok();
    }
}