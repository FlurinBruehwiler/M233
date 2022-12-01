using Punchclock.Models;
using Punchclock.Models.Dto;
using Punchclock.Services;

namespace Punchclock.EndpointDefinitions;

public class EntryEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/entries", GetAllEntries)
            .WithOpenApi();
        
        app.MapPost("/entries", CreateEntry)
            .AddEndpointFilter<ValidatorFilter<Entry>>()
            .WithOpenApi();
        
        app.MapDelete("/entries/{id:long}", DeleteEntry)
            .WithOpenApi();
        
        app.MapPut("/entries", PutEntry)
            .AddEndpointFilter<ValidatorFilter<Entry>>()
            .WithOpenApi();
    }

    private async Task<IResult> PutEntry(EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        var patchedEntry = await entryService.PutEntryAsync(entry);
        if (patchedEntry is null) return Results.BadRequest();
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new EntryDto { Id = patchedEntry.Id, CheckIn = patchedEntry.CheckIn, CheckOut = patchedEntry.CheckOut });
    }

    private async Task<IResult> DeleteEntry(long id, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        await entryService.DeleteEntryAsync(id);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> GetAllEntries(EntryService entryService)
    {
        return Results.Ok(await entryService.FindAllAsync());
    }

    private async Task<IResult> CreateEntry(EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        var createdEntry = await entryService.CreateEntry(entry);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new EntryDto { Id = createdEntry.Id, CheckIn = createdEntry.CheckIn, CheckOut = createdEntry.CheckOut });
    }
}