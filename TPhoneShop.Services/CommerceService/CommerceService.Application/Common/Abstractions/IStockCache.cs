namespace CommerceService.Application.Common.Abstractions
{
    public interface IStockCache
    {
        Task SyncAsync(Guid variantId, int stockQuantity);
        Task SyncManyAsync(IReadOnlyList<(Guid VariantId, int StockQuantity)> items);
        Task DecrementAsync(Guid variantId, int quantity);
        Task IncrementAsync(Guid variantId, int quantity);
    }
}
