using Microsoft.EntityFrameworkCore;
using Punchclock.Models;

namespace Punchclock.Services;

public class TagService
{
    private readonly PunchclockDbContext _punchclockDbContext;

    public TagService(PunchclockDbContext punchclockDbContext)
    {
        _punchclockDbContext = punchclockDbContext;
    }
    
    public Tag CreateTag(TagDto newTag)
    {
        var tag = new Tag
        {
            Title = newTag.Title
        };

        _punchclockDbContext.Tags.Add(tag);
        return tag;
    }

    public async Task<List<Tag>> FindAllAsync()
    {
        return await _punchclockDbContext.Tags.ToListAsync();
    }

    public async Task DeleteTagAsync(long id)
    {
        var tagToRemove = await _punchclockDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        if (tagToRemove is null)
            return;
        _punchclockDbContext.Tags.Remove(tagToRemove);
    }

    public async Task<Tag?> PatchTagAsync(TagDto patchedTag)
    {
        var tagToPatch = await _punchclockDbContext.Tags
            .FirstOrDefaultAsync(x => x.Id == patchedTag.Id);
        
        if (tagToPatch is null)
            return null;
        
        tagToPatch.Title = patchedTag.Title;

        return tagToPatch;
    }
}