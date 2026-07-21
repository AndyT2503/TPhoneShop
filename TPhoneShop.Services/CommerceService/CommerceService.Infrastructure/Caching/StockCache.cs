using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CommerceService.Infrastructure.Caching
{
    internal class StockCache(IConnectionMultiplexer redis, ILogger<StockCache> logger) : IStockCache
    {
        internal static string Key(Guid variantId) => $"stock:qty:{variantId}";

        public async Task SyncAsync(Guid variantId, int stockQuantity)
        {
            try
            {
                var db = redis.GetDatabase();
                await db.StringSetAsync(Key(variantId), stockQuantity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to sync stock for variant {VariantId}", variantId);
            }
        }

        public async Task SyncManyAsync(IReadOnlyList<(Guid VariantId, int StockQuantity)> items)
        {
            try
            {
                var db = redis.GetDatabase();
                var batch = db.CreateBatch();
                var tasks = items.Select(i => batch.StringSetAsync(Key(i.VariantId), i.StockQuantity)).ToList();
                batch.Execute();
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to sync stock batch");
            }
        }

        public async Task DecrementAsync(Guid variantId, int quantity)
        {
            try
            {
                var db = redis.GetDatabase();
                await db.StringDecrementAsync(Key(variantId), quantity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to decrement stock for variant {VariantId}", variantId);
            }
        }

        public async Task IncrementAsync(Guid variantId, int quantity)
        {
            try
            {
                var db = redis.GetDatabase();
                await db.StringIncrementAsync(Key(variantId), quantity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to increment stock for variant {VariantId}", variantId);
            }
        }
    }
}
