using Punchclock.Models;
using Punchclock.Services;

namespace Punchclock.EndpointDefinitions;

public class CategoryEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/entries", GetAllCategories);
        app.MapPost("/entries", CreateCategory);
        app.MapDelete("/entries/{id:long}", DeleteCategory);
        app.MapPatch("/entries", PatchCategory);
    }
    
    private async Task<IResult> PatchCategory(CategoryDto category, CategoryService categoryService, PunchclockDbContext punchclockDbContext)
    {
        var patchedCategory = await categoryService.PatchCategoryAsync(category);
        if (patchedCategory is null) return Results.BadRequest();
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new CategoryDto { Id = patchedCategory.Id,Title = patchedCategory.Title});
    }

    private async Task<IResult> DeleteCategory(long id, CategoryService categoryService, PunchclockDbContext punchclockDbContext)
    {
        await categoryService.DeleteCategoryAsync(id);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> GetAllCategories(CategoryService categoryService)
    {
        return Results.Ok(await categoryService.FindAllAsync());
    }

    private async Task<IResult> CreateCategory(CategoryDto category, CategoryService categoryService, PunchclockDbContext punchclockDbContext)
    {
        var createdCategory = categoryService.CreateCategory(category);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(new CategoryDto { Id = createdCategory.Id, Title = createdCategory.Title});
    }
}