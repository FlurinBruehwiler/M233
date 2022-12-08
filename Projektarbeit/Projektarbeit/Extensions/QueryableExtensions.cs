using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Projektarbeit.Errors;

namespace Projektarbeit.Extensions;

public static class QueryableExtensions
{
    public static async Task<TSource> FirstOrNotFoundAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var res = await source.FirstOrDefaultAsync(predicate, cancellationToken);

        if (res is null)
            throw new BadRequestException(
                new Error($"{nameof(TSource)}NotFound",$"The {nameof(TSource)} was not Found"));

        return res;
    }
}