using Microsoft.EntityFrameworkCore;

namespace Domain.Extensions;

public static class PaginateExtensions
{
    public static async Task<(List<T> entities, int count)> ToListPaginateAsync<T>(this IQueryable<T> query, int page, int pageSize)
    {

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var count = await query.CountAsync();

        return (entities, count);
    }
}
