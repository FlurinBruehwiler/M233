using Punchclock.Models;
using Punchclock.Models.Dto;
using Punchclock.Services;

namespace Punchclock.EndpointDefinitions;

public class TagEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/tags", GetAllTags)
            .WithOpenApi();
        
        app.MapPost("/tags", CreateTag)
            .AddEndpointFilter<ValidatorFilter<Tag>>()
            .WithOpenApi();
        
        app.MapDelete("/tags/{id:long}", DeleteTag)
            .WithOpenApi();
        
        app.MapPut("/tags", PutTag)
            .AddEndpointFilter<ValidatorFilter<Tag>>()
            .WithOpenApi();
    }
    
    private async Task<IResult> PutTag(TagDto entry, TagService tagService, PunchclockDbContext punchclockDbContext)
    {
        var patchedTag = await tagService.PutTagAsync(entry);
        if (patchedTag is null) return Results.BadRequest();
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new TagDto { Id = patchedTag.Id,Title = patchedTag.Title});
    }

    private async Task<IResult> DeleteTag(long id, TagService tagService, PunchclockDbContext punchclockDbContext)
    {
        await tagService.DeleteTagAsync(id);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> GetAllTags(TagService tagService)
    {
        return Results.Ok(await tagService.FindAllAsync());
    }

    private async Task<IResult> CreateTag(TagDto tag, TagService tagService, PunchclockDbContext punchclockDbContext)
    {
        var createdTag = tagService.CreateTag(tag);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new TagDto { Id = createdTag.Id, Title = createdTag.Title});
    }
}