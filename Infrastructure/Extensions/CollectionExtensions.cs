using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, object>> path) where T : class =>
            condition ? source.Include(path) : source;
    }
}
