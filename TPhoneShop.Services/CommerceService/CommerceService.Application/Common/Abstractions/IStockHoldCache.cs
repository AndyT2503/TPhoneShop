namespace CommerceService.Application.Common.Abstractions
{
    public interface IStockHoldCache
    {
        /// <summary>
        /// Attempts to hold stock for a list of items atomically.
        /// If any item cannot be held, all previously held items in this call are released.
        /// </summary>
        /// <param name="orderId">The order requesting the hold.</param>
        /// <param name="items">List of (variantId, requestedQty, currentDbStock) tuples.</param>
        /// <param name="holdDuration">How long to hold before expiry.</param>
        /// <returns>True if all items were held successfully; false if any item failed.</returns>
        Task<StockHoldResult> TryHoldAsync(
            Guid orderId,
            IReadOnlyList<StockHoldItem> items,
            TimeSpan holdDuration);

        /// <summary>
        /// Releases the stock hold for a specific order across all variants.
        /// Called when payment fails or hold expires.
        /// </summary>
        Task ReleaseAsync(Guid orderId, IReadOnlyList<Guid> variantIds);
    }

    public record StockHoldItem(Guid VariantId, int RequestedQuantity);

    public class StockHoldResult
    {
        public bool Success { get; init; }
        public bool RedisUnavailable { get; init; }
        public Guid? FailedVariantId { get; init; }
        public string? Reason { get; init; }

        public static StockHoldResult Succeeded() => new() { Success = true };

        public static StockHoldResult Failed(Guid variantId, string reason) => new()
        {
            Success = false,
            FailedVariantId = variantId,
            Reason = reason
        };

        public static StockHoldResult Unavailable() => new()
        {
            Success = false,
            RedisUnavailable = true
        };
    }
}
