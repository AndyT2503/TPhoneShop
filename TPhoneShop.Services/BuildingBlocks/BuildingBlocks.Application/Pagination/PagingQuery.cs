namespace BuildingBlocks.Application.Pagination
{
    public class PagingQuery
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
