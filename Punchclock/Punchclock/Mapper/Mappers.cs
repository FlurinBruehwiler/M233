using Punchclock.Models.Db;
using Punchclock.Models.Dto;

namespace Punchclock.Mapper;

public static class Mappers
{
    public static EntryDto ToDto(this Entry entry)
    {
        return new EntryDto
        {
            Id = entry.Id,
            Category = entry.CategoryId,
            CheckIn = entry.CheckIn,
            CheckOut = entry.CheckOut,
            Tags = entry.Tags.Select(y => y.Id).ToList()
        };
    }

    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Entries = category.Entries.Select(y => y.Id).ToList()
        };
    }

    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Title = tag.Title,
            Entries = tag.Entries.Select(y => y.Id).ToList()
        };
    }
}