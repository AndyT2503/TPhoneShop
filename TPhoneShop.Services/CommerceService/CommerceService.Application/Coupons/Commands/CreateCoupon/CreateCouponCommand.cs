namespace CommerceService.Application.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommand : IRequest<Guid>
    {
        public required string Code { get; set; }
        public required string DiscountType { get; set; }
        public required decimal DiscountValue { get; set; }
        public long? MaximumDiscountAmount { get; set; }
        public long? MinimumOrderAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int? PerUserUsageLimit { get; set; }
        public DateTimeOffset StartsAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}
