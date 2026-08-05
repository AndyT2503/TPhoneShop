namespace CommerceService.Application.Common.Abstractions
{
    public interface ICouponUsageCache
    {
        Task SyncAsync(Guid couponId, int currentUsage, int usageLimit, TimeSpan? ttl);
        Task IncrementUsageAsync(Guid couponId);
        Task DecrementUsageAsync(Guid couponId);
    }
}
