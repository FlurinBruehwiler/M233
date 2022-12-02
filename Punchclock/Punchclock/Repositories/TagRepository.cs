using Microsoft.EntityFrameworkCore;
using Punchclock.Errors;
using Punchclock.Models.Db;

namespace Punchclock.Repositories;

public class TagRepository
{
    private readonly PunchclockDbContext _punchclockDbContext;

    public TagRepository(PunchclockDbContext punchclockDbContext)
    {
        _punchclockDbContext = punchclockDbContext;
    }
    
    public async Task<Tag> GetTagById(long id)
    {
        var tag = await _punchclockDbContext.Tags
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (tag is null)
            throw new BadRequestException(Errors.Errors.TagNotFound);

        return tag;
    }
}