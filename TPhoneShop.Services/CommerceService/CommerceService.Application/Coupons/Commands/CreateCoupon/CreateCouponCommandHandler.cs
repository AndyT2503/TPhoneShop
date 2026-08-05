using CommerceService.Domain.Events.Coupon;
using CommerceService.Domain.ValueObjects;
using System.Text.Json;

namespace CommerceService.Application.Coupons.Commands.CreateCoupon
{
    internal class CreateCouponCommandHandler(CommerceDbContext dbContext) : IRequestHandler<CreateCouponCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var codeExists = await dbContext.Coupons
                .AnyAsync(c => c.Code == request.Code, cancellationToken);

            if (codeExists)
                throw new BadRequestException("Mã giảm giá đã tồn tại.");

            var coupon = new Coupon
            {
                Code = request.Code.Trim().ToUpperInvariant(),
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                MaximumDiscountAmount = request.MaximumDiscountAmount.HasValue
                    ? new Money(request.MaximumDiscountAmount.Value)
                    : null,
                MinimumOrderAmount = request.MinimumOrderAmount.HasValue
                    ? new Money(request.MinimumOrderAmount.Value)
                    : null,
                UsageLimit = request.UsageLimit,
                PerUserUsageLimit = request.PerUserUsageLimit,
                IsActive = true,
                StartsAt = request.StartsAt,
                ExpiresAt = request.ExpiresAt
            };

            dbContext.Coupons.Add(coupon);
            dbContext.OutboxMessages.Add(new OutboxMessage
            {
                Type = CouponCreatedEvent.EventName,
                Payload = JsonSerializer.SerializeToDocument(new CouponCreatedEvent(coupon.Id))
            });
            await dbContext.SaveChangesAsync(cancellationToken);

            return coupon.Id;
        }
    }
}
