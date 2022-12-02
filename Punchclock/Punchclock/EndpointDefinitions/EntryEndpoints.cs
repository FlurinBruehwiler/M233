using Punchclock.Mapper;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Services;
using Punchclock.Validators.ValidationFramework;

namespace Punchclock.EndpointDefinitions;

public class EntryEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/entries", GetAllEntries)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapPost("/entries", CreateEntry)
            .AddEndpointFilter<ValidatorFilter<EntryDto>>()
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapDelete("/entries/{id:long}", DeleteEntry)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapPut("/entries", PutEntry)
            .AddEndpointFilter<ValidatorFilter<EntryDto>>()
            .RequireAuthorization()
            .WithOpenApi();
    }

    private async Task<IResult> PutEntry(EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        var patchedEntry = await entryService.PutEntryAsync(entry);
        if (patchedEntry is null) return Results.BadRequest();
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(patchedEntry.ToDto());
    }

    private async Task<IResult> DeleteEntry(long id, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        await entryService.DeleteEntryAsync(id);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> GetAllEntries(EntryService entryService)
    {
        var entries = await entryService.FindAllAsync();
        return Results.Ok(entries.Select(x => x.ToDto()));
    }

    private async Task<IResult> CreateEntry(EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        var createdEntry = await entryService.CreateEntry(entry);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(createdEntry.ToDto());
    }
}