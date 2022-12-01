using Microsoft.AspNetCore.Mvc;
using Punchclock.Models.Dto;
using Punchclock.Services;

namespace Punchclock.EndpointDefinitions;

public class AuthenticationEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost( "/register", Register);
        app.MapPost( "/login", Login);
    }

    private async Task<IResult> Register(DtoAuthUser dtoAuthAuthUser, [FromServices] UserService userService, HttpResponse httpResponse)
    {
        await userService.RegisterUser(dtoAuthAuthUser, httpResponse);
        return Results.Ok();
    }
    
    private async Task<IResult> Login(DtoAuthUser dtoAuthUser, [FromServices] UserService userService, HttpResponse httpResponse)
    {
        await userService.LoginUser(dtoAuthUser, httpResponse);
        return Results.Ok();
    }
}