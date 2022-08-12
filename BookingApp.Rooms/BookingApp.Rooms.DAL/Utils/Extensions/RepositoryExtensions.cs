using BookingApp.Rooms.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Users.DAL.Utils.Extensions
{
    public static class RepositoriesExtensions
    {
        internal static async Task<PagedResults<T>> PaginateAsync<T>(this IQueryable<T> collection, Paginator paginator)
        {
            var count = await collection.CountAsync().ConfigureAwait(false);
            if (count == 0)
            {
                return new PagedResults<T>();
            }

            if (paginator == null)
            {
                paginator = new Paginator();
            }

            var totalPages = (int)Math.Ceiling((decimal)count / paginator.PageSize);
            var result = await collection
                .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                .Take(paginator.PageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResults<T>(result, paginator.CurrentPage, paginator.PageSize, totalPages, count);
        }
    }
}
