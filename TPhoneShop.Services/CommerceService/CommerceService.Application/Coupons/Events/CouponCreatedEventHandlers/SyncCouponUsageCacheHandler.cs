using BuildingBlocks.Application.Events;
using CommerceService.Application.Common.Abstractions;
using CommerceService.Domain.Events.Coupon;

namespace CommerceService.Application.Coupons.Events.CouponCreatedEventHandlers
{
    internal class SyncCouponUsageCacheHandler(
        CommerceDbContext dbContext,
        ICouponUsageCache couponUsageCache
    ) : INotificationHandler<DomainEventNotification<CouponCreatedEvent>>
    {
        public async Task Handle(DomainEventNotification<CouponCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var coupon = await dbContext.Coupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == notification.Event.CouponId, cancellationToken);

            if (coupon is null || !coupon.UsageLimit.HasValue)
                return;

            var ttl = coupon.ExpiresAt.HasValue
                ? coupon.ExpiresAt.Value - DateTimeOffset.UtcNow
                : (TimeSpan?)null;

            var currentUsage = await dbContext.OrderDiscounts
                .CountAsync(od => od.CouponId == coupon.Id, cancellationToken);

            await couponUsageCache.SyncAsync(coupon.Id, currentUsage, coupon.UsageLimit.Value, ttl);
        }
    }
}
