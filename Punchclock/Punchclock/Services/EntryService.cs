using Microsoft.EntityFrameworkCore;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Repositories;

namespace Punchclock.Services;

public class EntryService
{
    private readonly PunchclockDbContext _punchclockDbContext;
    private readonly UserService _userService;
    private readonly EntryRepository _entryRepository;
    private readonly CategoryRepository _categoryRepository;

    public EntryService(PunchclockDbContext punchclockDbContext, UserService userService,
        EntryRepository entryRepository, CategoryRepository categoryRepository)
    {
        _punchclockDbContext = punchclockDbContext;
        _userService = userService;
        _entryRepository = entryRepository;
        _categoryRepository = categoryRepository;
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
        var user = _userService.GetUser();
        return await _punchclockDbContext.Entries
            .Where(x => x.ApplicationUser == user)
            .Include(x => x.Tags)
            .ToListAsync();
    }

    public async Task DeleteEntryAsync(long id)
    {
        var entryToRemove = await _entryRepository.GetEntryById(id);
        
        _punchclockDbContext.Entries.Remove(entryToRemove);
    }

    public async Task<Entry?> PutEntryAsync(EntryDto patchedEntry)
    {
        var entryToPatch = await _entryRepository.GetEntryById(patchedEntry.Id);
        
        entryToPatch.CheckIn = patchedEntry.CheckIn;
        entryToPatch.CheckOut = patchedEntry.CheckOut;
        entryToPatch.Category = await _categoryRepository.GetCategoryById(patchedEntry.Category);

        return entryToPatch;
    }
}