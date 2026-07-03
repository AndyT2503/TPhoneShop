using BuildingBlocks.Application.Exceptions;

namespace BuildingBlocks.Application.Pagination
{
    public static class PaginationExtensions
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
    }
}
