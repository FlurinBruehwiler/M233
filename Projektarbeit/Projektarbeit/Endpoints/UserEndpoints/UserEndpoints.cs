using Projektarbeit.Endpoints.UserEndpoints.Dtos;
using Projektarbeit.Filters;
using Projektarbeit.Mappers;
using Projektarbeit.Models;
using Projektarbeit.Services;

namespace Projektarbeit.Endpoints.UserEndpoints;

public class UserEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users", GetUsers)
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
        
        app.MapPost("/users", CreateUser)
            .AddEndpointFilter<ValidatorFilter<CreateUserRequestDto>>()
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
        
        app.MapPatch("/users", PatchUser)
            .AddEndpointFilter<ValidatorFilter<PatchUserRequestDto>>()
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
        
        app.MapDelete("/users/{id:int}", DeleteUser)
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
    }

    private async Task<IResult> DeleteUser(int id, UserService userService, DatabaseContext databaseContext)
    {
        await userService.DeleteUser(id);
        await databaseContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> PatchUser(PatchUserRequestDto patchUserRequestDto, UserService userService, DatabaseContext databaseContext)
    {
        await userService.PatchUser(patchUserRequestDto);
        await databaseContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> CreateUser(CreateUserRequestDto createUserTo, UserService userService)
    {
        var createdUser = await userService.CreateUser(createUserTo);
        var dtoUser = createdUser.ToResponseDto();
        return Results.Created("/users", dtoUser);
    }

    private async Task<IResult> GetUsers(UserService userService)
    {
        var users = await userService.GetAllUsers();
        var dtoUsers = users.Select(x => x.ToResponseDto()).ToList();
        return Results.Ok(dtoUsers);
    }
}