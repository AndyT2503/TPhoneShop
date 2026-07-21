using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CommerceService.Infrastructure.Caching
{
    internal class CouponHoldCache(IConnectionMultiplexer redis, ILogger<CouponHoldCache> logger) : ICouponHoldCache
    {
        private static string HoldKey(Guid couponId) => $"coupon:hold:{couponId}";
        private static string ExpiryKey(Guid couponId) => $"coupon:hold:expiry:{couponId}";

        // KEYS[1] = coupon:usage:{couponId}, KEYS[2] = coupon:limit:{couponId},
        // KEYS[3] = coupon:hold:{couponId}, KEYS[4] = coupon:hold:expiry:{couponId}
        // ARGV[1] = orderId, ARGV[2] = expireAt
        // Returns: 1 = success, -1 = usage limit exceeded, -2 = keys not found (not cached)
        private const string HoldScript = """
            local usageKey = KEYS[1]
            local limitKey = KEYS[2]
            local holdKey = KEYS[3]
            local expiryKey = KEYS[4]
            local orderId = ARGV[1]
            local expireAt = tonumber(ARGV[2])

            local usageLimit = redis.call("GET", limitKey)
            if not usageLimit then
                return -2
            end
            usageLimit = tonumber(usageLimit)

            local currentUsage = tonumber(redis.call("GET", usageKey) or "0")
            local totalHolds = redis.call("HLEN", holdKey)

            if (currentUsage + totalHolds) >= usageLimit then
                return -1
            end

            redis.call("HSET", holdKey, orderId, "1")
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

        public async Task<CouponHoldResult> TryHoldAsync(Guid orderId, Guid couponId, TimeSpan holdDuration)
        {
            IDatabase db;
            try
            {
                db = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis connection unavailable for coupon hold.");
                return CouponHoldResult.Unavailable();
            }

            var expireAt = DateTimeOffset.UtcNow.Add(holdDuration).ToUnixTimeSeconds();
            var orderIdStr = orderId.ToString();

            var keys = new RedisKey[]
            {
                CouponUsageCache.UsageKey(couponId),
                CouponUsageCache.LimitKey(couponId),
                HoldKey(couponId),
                ExpiryKey(couponId)
            };

            var args = new RedisValue[] { orderIdStr, expireAt };

            try
            {
                var result = (long)await db.ScriptEvaluateAsync(HoldScript, keys, args);

                return result switch
                {
                    1 => CouponHoldResult.Succeeded(),
                    -1 => CouponHoldResult.Failed("Mã giảm giá đã hết lượt sử dụng."),
                    -2 => CouponHoldResult.Unavailable(),
                    _ => CouponHoldResult.Failed("Lỗi hệ thống khi giữ mã giảm giá.")
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis failed during coupon hold {CouponId} for order {OrderId}.", couponId, orderId);
                return CouponHoldResult.Unavailable();
            }
        }

        public async Task ReleaseAsync(Guid orderId, Guid couponId)
        {
            IDatabase db;
            try
            {
                db = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis connection unavailable for coupon release.");
                return;
            }

            var orderIdStr = orderId.ToString();
            var keys = new RedisKey[] { HoldKey(couponId), ExpiryKey(couponId) };
            var args = new RedisValue[] { orderIdStr };

            try
            {
                await db.ScriptEvaluateAsync(ReleaseScript, keys, args);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to release coupon hold {CouponId} for order {OrderId}", couponId, orderId);
            }
        }
    }
}
