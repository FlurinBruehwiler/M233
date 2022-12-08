using Projektarbeit.Endpoints.UserEndpoints.Dtos;
using Projektarbeit.Mappers;
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
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
        
        app.MapPatch("/users", PatchUser)
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
        
        app.MapDelete("/users/{id:int}", DeleteUser)
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
    }

    private async Task<IResult> DeleteUser(int id, UserService userService, SaveService saveService)
    {
        await userService.DeleteUser(id);
        await saveService.SaveChangesAndValidateAsync();
        return Results.Ok();
    }

    private async Task<IResult> PatchUser(PatchUserRequestDto patchUserRequestDto, UserService userService, SaveService saveService)
    {
        await userService.PatchUser(patchUserRequestDto);
        await saveService.SaveChangesAndValidateAsync();
        return Results.Ok();
    }

    private async Task<IResult> CreateUser(CreateUserRequestDto createUserTo, UserService userService, SaveService saveService)
    {
        var createdUser = await userService.CreateUser(createUserTo);
        await saveService.SaveChangesAndValidateAsync();
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