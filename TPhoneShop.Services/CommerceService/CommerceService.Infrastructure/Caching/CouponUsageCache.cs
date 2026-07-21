using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CommerceService.Infrastructure.Caching
{
    internal class CouponUsageCache(IConnectionMultiplexer redis, ILogger<CouponUsageCache> logger) : ICouponUsageCache
    {
        internal static string UsageKey(Guid couponId) => $"coupon:usage:{couponId}";
        internal static string LimitKey(Guid couponId) => $"coupon:limit:{couponId}";

        public async Task SyncAsync(Guid couponId, int currentUsage, int usageLimit, TimeSpan? ttl)
        {
            try
            {
                var db = redis.GetDatabase();
                await db.StringSetAsync(UsageKey(couponId), currentUsage, ttl);
                await db.StringSetAsync(LimitKey(couponId), usageLimit, ttl);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to sync coupon usage for {CouponId}", couponId);
            }
        }

        public async Task IncrementUsageAsync(Guid couponId)
        {
            try
            {
                var db = redis.GetDatabase();
                await db.StringIncrementAsync(UsageKey(couponId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to increment coupon usage for {CouponId}", couponId);
            }
        }

        public async Task DecrementUsageAsync(Guid couponId)
        {
            try
            {
                var db = redis.GetDatabase();
                await db.StringDecrementAsync(UsageKey(couponId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to decrement coupon usage for {CouponId}", couponId);
            }
        }
    }
}
