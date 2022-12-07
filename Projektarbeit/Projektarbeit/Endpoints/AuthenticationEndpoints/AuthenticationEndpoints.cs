using Microsoft.AspNetCore.Mvc;
using Projektarbeit.Endpoints.AuthenticationEndpoints.Dtos;
using Projektarbeit.Endpoints.UserEndpoints.Dtos;
using Projektarbeit.Filters;
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
            .AddEndpointFilter<ValidatorFilter<RegisterRequestDto>>()
            .WithOpenApi();
    }

    private async Task<IResult> Register(RegisterRequestDto registerRequestDtoAuth, [FromServices] UserService userService, DatabaseContext punchclockDbContext)
    {
        await userService.RegisterUser(registerRequestDtoAuth);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }
    
    private async Task<IResult> Login(LoginRequestDto registerRequestDto, [FromServices] UserService userService)
    {
        await userService.LoginUser(registerRequestDto);
        return Results.Ok();
    }
}