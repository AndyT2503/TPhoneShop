namespace CommerceService.Application.Common.Abstractions
{
    public interface ICouponHoldCache
    {
        Task<CouponHoldResult> TryHoldAsync(Guid orderId, Guid couponId, TimeSpan holdDuration);
        Task ReleaseAsync(Guid orderId, Guid couponId);
    }

    public class CouponHoldResult
    {
        public bool Success { get; init; }
        public bool RedisUnavailable { get; init; }
        public string? Reason { get; init; }

        public static CouponHoldResult Succeeded() => new() { Success = true };

        public static CouponHoldResult Failed(string reason) => new()
        {
            Success = false,
            Reason = reason
        };

        public static CouponHoldResult Unavailable() => new()
        {
            Success = false,
            RedisUnavailable = true
        };
    }
}
