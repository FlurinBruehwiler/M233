using Microsoft.EntityFrameworkCore;
using Punchclock.Errors;
using Punchclock.Models.Db;

namespace Punchclock.Repositories;

public class CategoryRepository
{
    private readonly PunchclockDbContext _punchclockDbContext;

    public CategoryRepository(PunchclockDbContext punchclockDbContext)
    {
        _punchclockDbContext = punchclockDbContext;
    }
    
    public async Task<Category> GetCategoryById(long id)
    {
        var category = await _punchclockDbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (category is null)
            throw new BadRequestException(Errors.Errors.CategoryNotFound);

        return category;
    }
}