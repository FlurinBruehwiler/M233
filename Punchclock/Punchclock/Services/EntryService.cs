using Microsoft.EntityFrameworkCore;
using Punchclock.Models;
using Punchclock.Models.Dto;

namespace Punchclock.Services;

public class EntryService
{
    private readonly PunchclockDbContext _punchclockDbContext;

    public EntryService(PunchclockDbContext punchclockDbContext)
    {
        _punchclockDbContext = punchclockDbContext;
    }

    public async Task<Entry> CreateEntry(EntryDto newEntry)
    {
        var entry = new Entry
        {
            CheckIn = newEntry.CheckIn,
            CheckOut = newEntry.CheckOut,
            Category = await _punchclockDbContext.Categories.FirstAsync(x => x.Id == newEntry.Category),
            ApplicationUser = null
        };
        
        _punchclockDbContext.Entries.Add(entry);
        return entry;
    }

    public async Task<List<Entry>> FindAllAsync()
    {
        return await _punchclockDbContext.Entries.ToListAsync();
    }

    public async Task DeleteEntryAsync(long id)
    {
        var entryToRemove = await _punchclockDbContext.Entries.FirstOrDefaultAsync(x => x.Id == id);
        if (entryToRemove is null)
            return;
        _punchclockDbContext.Entries.Remove(entryToRemove);
    }

    public async Task<Entry?> PutEntryAsync(EntryDto patchedEntry)
    {
        var entryToPatch = await _punchclockDbContext.Entries.FirstOrDefaultAsync(x => x.Id == patchedEntry.Id);
        if (entryToPatch is null)
            return null;
        
        entryToPatch.CheckIn = patchedEntry.CheckIn;
        entryToPatch.CheckOut = patchedEntry.CheckOut;

        return entryToPatch;
    }
}