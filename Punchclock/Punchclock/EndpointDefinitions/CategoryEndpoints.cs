using Punchclock.Mapper;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Services;
using Punchclock.Validators.ValidationFramework;

namespace Punchclock.EndpointDefinitions;

public class CategoryEndpoints : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/categories", GetAllCategories)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapPost("/categories", CreateCategory)
            .AddEndpointFilter<ValidatorFilter<CategoryDto>>()
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapDelete("/categories/{id:long}", DeleteCategory)
            .RequireAuthorization()
            .WithOpenApi();
        
        app.MapPut("/categories", PutCategory)
            .AddEndpointFilter<ValidatorFilter<CategoryDto>>()
            .RequireAuthorization()
            .WithOpenApi();
    }
    
    private async Task<IResult> PutCategory(CategoryDto category, CategoryService categoryService, PunchclockDbContext punchclockDbContext)
    {
        var patchedCategory = await categoryService.PutCategoryAsync(category);
        if (patchedCategory is null) return Results.BadRequest();
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(patchedCategory.ToDto());
    }

    private async Task<IResult> DeleteCategory(long id, CategoryService categoryService, PunchclockDbContext punchclockDbContext)
    {
        await categoryService.DeleteCategoryAsync(id);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok();
    }

    private async Task<IResult> GetAllCategories(CategoryService categoryService)
    {
        var categories = await categoryService.FindAllAsync();
        return Results.Ok(categories.Select(x => x.ToDto()).ToList());
    }

    private async Task<IResult> CreateCategory(CategoryDto category, CategoryService categoryService, PunchclockDbContext punchclockDbContext)
    {
        var createdCategory = categoryService.CreateCategory(category);
        await punchclockDbContext.SaveChangesAsync();
        return Results.Ok(createdCategory.ToDto());
    }
}