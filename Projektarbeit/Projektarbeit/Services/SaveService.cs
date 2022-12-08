using Microsoft.EntityFrameworkCore;
using Projektarbeit.Errors;
using Projektarbeit.Models;
using Projektarbeit.Validators;

namespace Projektarbeit.Services;

public class SaveService
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserService _userService;
    // public static Dictionary<Type, List<ICustomValidator<object>>> Validators { get; set; } = null!;

    public SaveService(DatabaseContext databaseContext, UserService userService)
    {
        _databaseContext = databaseContext;
        _userService = userService;
    }

    public async Task SaveChangesAndValidateAsync()
    {
        var res = await ValidateChangedObjects();

        if (res.Count > 0)
        {
            throw new BadRequestException(res);
        }
        
        await _databaseContext.SaveChangesAsync();
    }
    
    private async Task<List<Error>> ValidateChangedObjects()
    {
        List<Error> errors = new();

        // _databaseContext.ChangeTracker.DetectChanges();
        // var user = await _userService.GetUser();
        //
        // foreach (var entry in _databaseContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added))
        // {
        //     var changedProperties = entry.Members.AsEnumerable()
        //         .Where(x => x.IsModified)
        //         .Select(x => x.Metadata.Name).ToList();
        //
        //     changedProperties.AddRange(entry.Collections.AsEnumerable()
        //         .Where(x => x.IsModified)
        //         .Select(x => x.Metadata.Name).ToList());
        //
        //     errors.AddRange(Validators[entry.Entity.GetType()]
        //         .SelectMany(x => x.Validate(entry, changedProperties, user).ToList()));
        // }

        return errors;
    }
}