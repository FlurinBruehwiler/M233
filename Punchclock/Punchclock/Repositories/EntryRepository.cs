using Microsoft.EntityFrameworkCore;
using Punchclock.Errors;
using Punchclock.Models.Db;
using Punchclock.Services;

namespace Punchclock.Repositories;

public class EntryRepository
{
    private readonly PunchclockDbContext _punchclockDbContext;
    private readonly UserService _userService;

    public EntryRepository(PunchclockDbContext punchclockDbContext, UserService userService)
    {
        _punchclockDbContext = punchclockDbContext;
        _userService = userService;
    }
    
    public async Task<Entry> GetEntryById(long id)
    {
        var entry = await _punchclockDbContext.Entries
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (entry is null)
            throw new BadRequestException(Errors.Errors.EntryNotFound);
        
        if (entry.ApplicationUserId != _userService.GetUser().Id)
            throw new BadRequestException(Errors.Errors.UserHasNoRights);

        return entry;
    }
}