using Microsoft.EntityFrameworkCore;
using Projektarbeit.Errors;
using Projektarbeit.Models;
using Projektarbeit.Validators;

namespace Projektarbeit.Services;

public class SaveService
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserService _userService;
    private readonly Dictionary<Type, List<IValidator<object>>> _validators;

    public SaveService(DatabaseContext databaseContext, UserService userService)
    {
        _databaseContext = databaseContext;
        _userService = userService;

        _validators = typeof(IValidator<>)
            .Assembly
            .ExportedTypes
            .Where(x => typeof(IValidator<>)
                            .IsAssignableFrom(x)
                        && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IValidator<object>>()
            .GroupBy(x => x.GetType())
            .ToDictionary(x => x.Key
                    .GetGenericArguments()
                    .First(),
                x => x.AsEnumerable().ToList());
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

        _databaseContext.ChangeTracker.DetectChanges();
        var user = await _userService.GetUser();

        foreach (var entry in _databaseContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
        {
            var changedProperties = entry.Members.AsEnumerable()
                .Where(x => x.IsModified)
                .Select(x => x.Metadata.Name).ToList();

            changedProperties.AddRange(entry.Collections.AsEnumerable()
                .Where(x => x.IsModified)
                .Select(x => x.Metadata.Name).ToList());

            errors.AddRange(_validators[entry.Entity.GetType()]
                .SelectMany(x => x.Validate(entry, changedProperties, user).ToList()));
        }

        return errors;
    }
}