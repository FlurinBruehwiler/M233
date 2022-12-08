using Projektarbeit.Endpoints.UserEndpoints;
using Projektarbeit.Errors;
using Projektarbeit.Models;

namespace Projektarbeit.Validators;

// public class UserCustomValidator : ICustomValidator<User>
// {
//     public IEnumerable<Error> Validate(User obj, List<string> changedProperties, User actingUser)
//     {
//         var validator = new FluentUserValidator();
//         var res = validator.Validate(obj);
//         if (!res.IsValid)
//             yield return new Error("ValidationFail", res.ToString());
//     }
// }