using Projektarbeit.Services;

namespace Projektarbeit.Endpoints;

public class AdministrationEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPut("/workplace/{count:int}", PutWorkplace)
            .RequireAuthorization(AuthService.AdministratorRole)
            .WithOpenApi();
        
        app.MapGet("/workplace", GetWorkplace)
            .RequireAuthorization()
            .WithOpenApi();
    }

    private IResult PutWorkplace(int count, IConfiguration configuration)
    {
        configuration.GetRequiredSection("Workplace:Amount").Value = count.ToString();
        return Results.Ok();
    }
    
    private IResult GetWorkplace(IConfiguration configuration)
    {
        var value = configuration.GetRequiredSection("Workplace:Amount").Value;
        return Results.Ok(value);
    }
}