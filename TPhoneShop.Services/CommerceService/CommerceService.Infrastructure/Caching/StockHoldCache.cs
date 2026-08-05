using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CommerceService.Infrastructure.Caching
{
    internal class StockHoldCache(IConnectionMultiplexer redis, ILogger<StockHoldCache> logger) : IStockHoldCache
    {
        private static string HoldKey(Guid variantId) => $"stock:hold:variant:{variantId}";
        private static string ExpiryKey(Guid variantId) => $"stock:hold:expiry:{variantId}";

        // KEYS[1] = stock qty key, KEYS[2] = hold hash key, KEYS[3] = expiry sorted set key
        // ARGV[1] = orderId, ARGV[2] = requestedQty, ARGV[3] = expireAt
        // Returns: 1 = success, -1 = insufficient stock, -2 = stock key not found
        private const string HoldScript = """
            local stockKey = KEYS[1]
            local holdKey = KEYS[2]
            local expiryKey = KEYS[3]
            local orderId = ARGV[1]
            local requestedQty = tonumber(ARGV[2])
            local expireAt = tonumber(ARGV[3])

            local currentStock = redis.call("GET", stockKey)
            if not currentStock then
                return -2
            end
            currentStock = tonumber(currentStock)

            local holds = redis.call("HVALS", holdKey)
            local totalHold = 0
            for _, v in ipairs(holds) do
                totalHold = totalHold + tonumber(v)
            end

            local available = currentStock - totalHold
            if available < requestedQty then
                return -1
            end

            redis.call("HSET", holdKey, orderId, requestedQty)
            redis.call("ZADD", expiryKey, expireAt, orderId)
            return 1
            """;

        private const string ReleaseScript = """
            local holdKey = KEYS[1]
            local expiryKey = KEYS[2]
            local orderId = ARGV[1]

            redis.call("HDEL", holdKey, orderId)
            redis.call("ZREM", expiryKey, orderId)
            return 1
            """;

        public async Task<StockHoldResult> TryHoldAsync(
            Guid orderId,
            IReadOnlyList<StockHoldItem> items,
            TimeSpan holdDuration)
        {
            IDatabase db;
            try
            {
                db = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis connection unavailable for stock hold.");
                return StockHoldResult.Unavailable();
            }

            var expireAt = DateTimeOffset.UtcNow.Add(holdDuration).ToUnixTimeSeconds();
            var orderIdStr = orderId.ToString();
            var heldVariants = new List<Guid>();

            foreach (var item in items)
            {
                var keys = new RedisKey[]
                {
                    StockCache.Key(item.VariantId),
                    HoldKey(item.VariantId),
                    ExpiryKey(item.VariantId)
                };
                var args = new RedisValue[] { orderIdStr, item.RequestedQuantity, expireAt };

                try
                {
                    var result = (long)await db.ScriptEvaluateAsync(HoldScript, keys, args);

                    if (result == -2)
                    {
                        await ReleaseAsync(orderId, heldVariants);
                        return StockHoldResult.Unavailable();
                    }

                    if (result == -1)
                    {
                        await ReleaseAsync(orderId, heldVariants);
                        return StockHoldResult.Failed(item.VariantId, "Sản phẩm không đủ tồn kho.");
                    }

                    heldVariants.Add(item.VariantId);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Redis failed during hold for variant {VariantId}, order {OrderId}.", item.VariantId, orderId);
                    await ReleaseAsync(orderId, heldVariants);
                    return StockHoldResult.Unavailable();
                }
            }

            return StockHoldResult.Succeeded();
        }

        public async Task ReleaseAsync(Guid orderId, IReadOnlyList<Guid> variantIds)
        {
            IDatabase db;
            try
            {
                db = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis connection unavailable for stock release.");
                return;
            }

            var orderIdStr = orderId.ToString();

            foreach (var variantId in variantIds)
            {
                var keys = new RedisKey[] { HoldKey(variantId), ExpiryKey(variantId) };
                var args = new RedisValue[] { orderIdStr };

                try
                {
                    await db.ScriptEvaluateAsync(ReleaseScript, keys, args);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to release stock hold for variant {VariantId}, order {OrderId}", variantId, orderId);
                }
            }
        }
    }
}
