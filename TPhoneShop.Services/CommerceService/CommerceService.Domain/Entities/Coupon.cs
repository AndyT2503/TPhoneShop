using BuildingBlocks.Domain.Exceptions;
using CommerceService.Domain.Constants;
using CommerceService.Domain.ValueObjects;

namespace CommerceService.Domain.Entities
{
    public class Coupon : BaseEntity, ISoftDeletable
    {
        public required string Code { get; set; }
        /// <summary>
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.DiscountTypes"/>.
        /// </summary>
        public required string DiscountType { get; set; }
        public required decimal DiscountValue { get; set; }
        public Money? MaximumDiscountAmount { get; set; }
        public Money? MinimumOrderAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int? PerUserUsageLimit { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset StartsAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

        public Money CalculateDiscountAmount(Money orderAmount, int totalUsage, int userUsage)
        {
            // Validate

            if (!IsActive)
                throw new DomainException("Mã giảm giá đã bị vô hiệu hóa.");

            var now = DateTimeOffset.UtcNow;

            if (StartsAt > now)
                throw new DomainException("Mã giảm giá chưa đến thời gian sử dụng.");

            if (ExpiresAt.HasValue && ExpiresAt.Value <= now)
                throw new DomainException("Mã giảm giá đã hết hạn.");

            if (MinimumOrderAmount is not null &&
                orderAmount < MinimumOrderAmount)
            {
                throw new DomainException("Đơn hàng chưa đạt giá trị tối thiểu.");
            }

            if (UsageLimit.HasValue &&
                totalUsage >= UsageLimit.Value)
            {
                throw new DomainException("Mã giảm giá đã hết lượt sử dụng.");
            }

            if (PerUserUsageLimit.HasValue &&
                userUsage >= PerUserUsageLimit.Value)
            {
                throw new DomainException("Bạn đã vượt quá số lần sử dụng mã giảm giá.");
            }

            // Calculate

            Money discount = DiscountType switch
            {
                DiscountTypes.FixedAmount =>
                    new Money((long)DiscountValue, orderAmount.Currency),

                DiscountTypes.Percentage =>
                    new Money(
                        (long)Math.Round(
                            orderAmount.Amount * DiscountValue / 100m,
                            MidpointRounding.AwayFromZero),
                        orderAmount.Currency),

                _ => throw new DomainException("Loại giảm giá không hợp lệ.")
            };

            if (MaximumDiscountAmount is not null &&
                discount > MaximumDiscountAmount)
            {
                discount = MaximumDiscountAmount;
            }

            if (discount > orderAmount)
            {
                discount = orderAmount;
            }

            return discount;
        }
    }
}