using BuildingBlocks.Application.Exceptions;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new BadRequestException("Số trang cần lớn hơn 0.");
            if (pageSize <= 0)
                throw new BadRequestException("Số bản ghi cần lớn hơn 0.");
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}
