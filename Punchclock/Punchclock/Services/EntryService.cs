using Microsoft.EntityFrameworkCore;
using Punchclock.Models;
using Punchclock.Models.Dto;

namespace Punchclock.Services;

public class EntryService
{
    private readonly PunchclockDbContext _punchclockDbContext;
    private readonly UserService _userService;

    public EntryService(PunchclockDbContext punchclockDbContext, UserService userService)
    {
        _punchclockDbContext = punchclockDbContext;
        _userService = userService;
    }

    public async Task<Entry> CreateEntry(EntryDto newEntry)
    {
        var entry = new Entry
        {
            CheckIn = newEntry.CheckIn,
            CheckOut = newEntry.CheckOut,
            Category = await _punchclockDbContext.Categories.FirstAsync(x => x.Id == newEntry.Category),
            ApplicationUser = _userService.GetUser()
        };
        
        _punchclockDbContext.Entries.Add(entry);
        return entry;
    }

    public async Task<List<Entry>> FindAllAsync()
    {
        return await _punchclockDbContext.Entries
            .Where(x => x.ApplicationUser == _userService.GetUser())
            .ToListAsync();
    }

    public async Task DeleteEntryAsync(long id)
    {
        var entryToRemove = await _punchclockDbContext.Entries.FirstOrDefaultAsync(x => x.Id == id);
        if (entryToRemove is null)
            return;

        if (entryToRemove.ApplicationUser != _userService.GetUser())
            return;
        
        _punchclockDbContext.Entries.Remove(entryToRemove);
    }

    public async Task<Entry?> PutEntryAsync(EntryDto patchedEntry)
    {
        var entryToPatch = await _punchclockDbContext.Entries.FirstOrDefaultAsync(x => x.Id == patchedEntry.Id);
        if (entryToPatch is null)
            return null;
        
        if (entryToPatch.ApplicationUser != _userService.GetUser())
            return null;
        
        entryToPatch.CheckIn = patchedEntry.CheckIn;
        entryToPatch.CheckOut = patchedEntry.CheckOut;

        return entryToPatch;
    }
}