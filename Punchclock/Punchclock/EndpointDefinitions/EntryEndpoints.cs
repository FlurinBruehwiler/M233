using Punchclock.Models;
using Punchclock.Services;

namespace Punchclock.EndpointDefinitions;

public class EntryEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/entries", GetAllEntries);
        app.MapPost("/entries", CreateEntry);
        app.MapDelete("/entries/{id:long}", DeleteEntry);
        app.MapPatch("/entries", PatchEntry);
    }

    private async Task<IResult> PatchEntry(EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext)
    {
        var patchedEntry = await entryService.PatchEntryAsync(entry);
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
        var createdEntry = entryService.CreateEntry(entry);
        if (createdEntry is null) return Results.BadRequest();
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new EntryDto { Id = createdEntry.Id, CheckIn = createdEntry.CheckIn, CheckOut = createdEntry.CheckOut });
    }
}