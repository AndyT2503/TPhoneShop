using CommerceService.Domain.ValueObjects;

namespace CommerceService.Domain.Entities
{
    public class OrderDiscount : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;
        public required string Code { get; set; }
        public required string DiscountType { get; set; }
        public required decimal DiscountValue { get; set; }
        public required Money AppliedAmount { get; set; }
    }
}