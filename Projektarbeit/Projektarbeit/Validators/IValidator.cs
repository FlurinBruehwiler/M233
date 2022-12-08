using Projektarbeit.Errors;
using Projektarbeit.Models;

namespace Projektarbeit.Validators;

public interface IValidator<in T> where T : class
{
    public IEnumerable<Error> Validate(T obj, List<string> changedProperties, User actingUser);
}