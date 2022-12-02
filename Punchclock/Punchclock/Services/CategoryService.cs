using Microsoft.EntityFrameworkCore;
using Punchclock.Models;
using Punchclock.Models.Dto;

namespace Punchclock.Services;

public class CategoryService
{
    private readonly PunchclockDbContext _punchclockDbContext;

    public CategoryService(PunchclockDbContext punchclockDbContext)
    {
        _punchclockDbContext = punchclockDbContext;
    }
    
    public Category CreateCategory(CategoryDto newCategory)
    {
        var category = new Category
        {
            Title = newCategory.Title
        };

        _punchclockDbContext.Categories.Add(category);
        return category;
    }

    public async Task<List<Category>> FindAllAsync()
    {
        return await _punchclockDbContext.Categories.ToListAsync();
    }

    public async Task DeleteCategoryAsync(long id)
    {
        var categoryToRemove = await _punchclockDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToRemove is null)
            throw new BadRequestException(Errors.CategoryNotFound);
            
        _punchclockDbContext.Categories.Remove(categoryToRemove);
    }

    public async Task<Category?> PutCategoryAsync(CategoryDto patchedCategory)
    {
        var categoryToPatch = await _punchclockDbContext.Categories.FirstOrDefaultAsync(x => x.Id == patchedCategory.Id);
        if (categoryToPatch is null)
            throw new BadRequestException(Errors.CategoryNotFound);
        
        categoryToPatch.Title = patchedCategory.Title;

        return categoryToPatch;
    }
}