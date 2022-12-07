using Microsoft.AspNetCore.Mvc;
using Projektarbeit.Models;
using Projektarbeit.Services;

namespace Projektarbeit.Endpoints.AuthenticationEndpoints;

public class AuthenticationEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/login", Login)
            .WithOpenApi();
        
        app.MapPost("/register", Register)
            .WithOpenApi();
    }

    private async Task<IResult> Register(UserDto userDtoAuth, [FromServices] UserService userService, DatabaseContext punchclockDbContext)
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