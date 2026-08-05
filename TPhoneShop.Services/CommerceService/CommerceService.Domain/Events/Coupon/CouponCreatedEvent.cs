using BuildingBlocks.Domain.Events;

namespace CommerceService.Domain.Events.Coupon
{
    public record CouponCreatedEvent(Guid CouponId) : IDomainEvent
    {
        public const string EventName = "coupon_created_event";
    }
}
