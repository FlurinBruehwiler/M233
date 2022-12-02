using Microsoft.EntityFrameworkCore;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Repositories;

namespace Punchclock.Services;

public class TagService
{
    private readonly PunchclockDbContext _punchclockDbContext;
    private readonly TagRepository _tagRepository;

    public TagService(PunchclockDbContext punchclockDbContext, TagRepository tagRepository)
    {
        _punchclockDbContext = punchclockDbContext;
        _tagRepository = tagRepository;
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
        return await _punchclockDbContext.Tags
            .Include(x => x.Entries)
            .ToListAsync();
    }

    public async Task DeleteTagAsync(long id)
    {
        var tagToRemove = await _tagRepository.GetTagById(id);
        _punchclockDbContext.Tags.Remove(tagToRemove);
    }

    public async Task<Tag?> PutTagAsync(TagDto patchedTag)
    {
        var tagToPatch = await _tagRepository.GetTagById(patchedTag.Id);
        
        tagToPatch.Title = patchedTag.Title;

        return tagToPatch;
    }
}