namespace BuildingBlocks.Application.Pagination
{
    public class PagingResponse<T>
    {
        public int TotalCount { get; set; }
        public required IReadOnlyCollection<T> Items { get; set; }
    }
}
