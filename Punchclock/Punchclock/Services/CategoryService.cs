using Microsoft.EntityFrameworkCore;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Repositories;

namespace Punchclock.Services;

public class CategoryService
{
    private readonly PunchclockDbContext _punchclockDbContext;
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(PunchclockDbContext punchclockDbContext, CategoryRepository categoryRepository)
    {
        _punchclockDbContext = punchclockDbContext;
        _categoryRepository = categoryRepository;
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
        return await _punchclockDbContext.Categories
            .Include(x => x.Entries)
            .ToListAsync();
    }

    public async Task DeleteCategoryAsync(long id)
    {
        var categoryToRemove = await _categoryRepository.GetCategoryById(id);
            
        _punchclockDbContext.Categories.Remove(categoryToRemove);
    }

    public async Task<Category?> PutCategoryAsync(CategoryDto patchedCategory)
    {
        var categoryToPatch = await _categoryRepository.GetCategoryById(patchedCategory.Id);
        
        categoryToPatch.Title = patchedCategory.Title;

        return categoryToPatch;
    }
}