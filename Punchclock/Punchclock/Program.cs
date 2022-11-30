using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Punchclock;
using Punchclock.Models;
using Punchclock.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EntryValidator>();
builder.Services.AddScoped<EntryService>();
builder.Services.AddDbContext<PunchclockDbContext>(options =>
{
    options.UseSqlite("db.sql");
    options.EnableSensitiveDataLogging();
});

var app = builder.Build();

app.MapGet("/entries", async (EntryService entryService)
    => Results.Ok(await entryService.FindAllAsync()));

app.MapPost("/entries", async (EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext)
    =>
{
    var createdEntry = await entryService.CreateEntryAsync(entry);
    if (createdEntry is null)
        return Results.BadRequest();
    await punchclockDbContext.SaveChangesAsync();
    return Results.Ok(new EntryDto
    {
        Id = createdEntry.Id,
        CheckIn = createdEntry.CheckIn,
        CheckOut = createdEntry.CheckOut
    });
});

app.MapDelete("/entries/{id:long}",
    async (long id, EntryService entryService, PunchclockDbContext punchclockDbContext) =>
    {
        await entryService.DeleteEntryAsync(id);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    });

app.MapPatch("/entries", async (EntryDto entry, EntryService entryService, PunchclockDbContext punchclockDbContext) =>
{
    var patchedEntry = await entryService.PatchEntryAsync(entry);
    if (patchedEntry is null)
        return Results.BadRequest();
    await punchclockDbContext.SaveChangesAsync();
    return Results.Ok(new EntryDto
    {
        Id = patchedEntry.Id,
        CheckIn = patchedEntry.CheckIn,
        CheckOut = patchedEntry.CheckOut
    });
});

app.Run();

public partial class Program
{
}