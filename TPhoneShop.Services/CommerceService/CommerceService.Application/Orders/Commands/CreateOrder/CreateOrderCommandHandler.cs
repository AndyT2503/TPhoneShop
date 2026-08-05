using BuildingBlocks.Application.Auth;
using CommerceService.Application.Common.Abstractions;
using CommerceService.Application.Orders.Commands.Dtos;
using CommerceService.Domain.Constants;
using CommerceService.Domain.Events.Order;
using CommerceService.Domain.ValueObjects;
using System.Text.Json;

namespace CommerceService.Application.Orders.Commands.CreateOrder
{
    internal class CreateOrderCommandHandler(
        CommerceDbContext dbContext,
        ICurrentUser currentUser,
        IStockHoldCache stockHoldCache,
        ICouponHoldCache couponHoldCache,
        IShippingFeeCalculator shippingFeeCalculator,
        IOrderNumberGenerator orderNumberGenerator,
        IIdempotencyCache idempotencyCache
    ) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private static readonly TimeSpan HoldDuration = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan IdempotencyExpiration = TimeSpan.FromHours(24);

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var existingResult = await idempotencyCache.GetExistingResultAsync<CreateOrderResponse>(request.IdempotencyKey);
            if (existingResult is not null)
                return existingResult;

            var customerId = currentUser.Id
                ?? throw new UnauthorizedException("Bạn cần đăng nhập để đặt hàng.");

            var variants = await LoadAndValidateVariants(request, cancellationToken);
            var shippingAddress = MapShippingAddress(request);
            var shippingFee = shippingFeeCalculator.Calculate(request.ShippingMethod, shippingAddress);
            var orderItems = BuildOrderItems(request, variants);

            var order = new Order(
                customerId,
                orderNumberGenerator.Generate(),
                request.PaymentMethod,
                request.ShippingMethod,
                shippingAddress,
                orderItems,
                request.CustomerNote
            );

            order.UpdateShippingFee(shippingFee);

            var variantIds = request.Items.Select(x => x.ProductVariantId).ToList();
            await HoldStock(order.Id, request, variants, cancellationToken);

            var heldCoupons = new List<Guid>();

            try
            {
                var discounts = await ValidateAndHoldCoupons(
                    order.Id, customerId, request, variants, heldCoupons, cancellationToken);

                foreach (var discount in discounts)
                {
                    order.ApplyDiscount(discount);
                }

                Persist(order, customerId);
                order.AddLog(OrderLogAction.OrderStatusChange, customerId);
                await dbContext.SaveChangesAsync(cancellationToken);

                var response = new CreateOrderResponse
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    TotalAmount = order.TotalAmount.Amount
                };

                await idempotencyCache.SaveResultAsync(request.IdempotencyKey, response, IdempotencyExpiration);

                return response;
            }
            catch
            {
                await ReleaseAllHolds(order.Id, variantIds, heldCoupons);
                throw;
            }
        }

        private async Task<List<ProductVariant>> LoadAndValidateVariants(
            CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var variantIds = request.Items.Select(x => x.ProductVariantId).ToList();

            var variants = await dbContext.ProductVariants
                .AsNoTracking()
                .Include(v => v.Product)
                .Where(v => variantIds.Contains(v.Id) && v.IsActive && v.Product.IsActive)
                .ToListAsync(cancellationToken);

            if (variants.Count != variantIds.Count)
                throw new BadRequestException("Một hoặc nhiều sản phẩm không tồn tại hoặc đã ngừng kinh doanh.");

            return variants;
        }

        private async Task HoldStock(Guid orderId, CreateOrderCommand request, List<ProductVariant> variants, CancellationToken cancellationToken)
        {
            var holdItems = request.Items.Select(item =>
                new StockHoldItem(item.ProductVariantId, item.Quantity)).ToList();

            var result = await stockHoldCache.TryHoldAsync(orderId, holdItems, HoldDuration);

            if (result.Success)
                return;

            if (result.RedisUnavailable)
                throw new ServiceUnavailableException("Hệ thống đang tải, vui lòng thử lại sau.");

            throw new BadRequestException(result.Reason!);
        }

        private async Task<List<OrderDiscount>> ValidateAndHoldCoupons(
            Guid orderId,
            Guid customerId,
            CreateOrderCommand request,
            List<ProductVariant> variants,
            List<Guid> heldCoupons,
            CancellationToken cancellationToken)
        {
            if (request.CouponCodes.Count == 0)
                return [];

            var coupons = await dbContext.Coupons
                .AsNoTracking()
                .Where(c => request.CouponCodes.Contains(c.Code) && c.IsActive)
                .ToListAsync(cancellationToken);

            if (coupons.Count != request.CouponCodes.Count)
                throw new BadRequestException("Một hoặc nhiều mã giảm giá không hợp lệ.");

            var subtotal = CalculateSubtotal(request, variants);
            var discounts = new List<OrderDiscount>();

            foreach (var coupon in coupons)
            {
                var userUsage = await dbContext.OrderDiscounts
                    .CountAsync(od => od.CouponId == coupon.Id && od.Order.CustomerId == customerId, cancellationToken);

                if (coupon.PerUserUsageLimit.HasValue && userUsage >= coupon.PerUserUsageLimit.Value)
                    throw new BadRequestException("Bạn đã vượt quá số lần sử dụng mã giảm giá.");

                if (coupon.UsageLimit.HasValue)
                {
                    var holdResult = await couponHoldCache.TryHoldAsync(orderId, coupon.Id, HoldDuration);

                    if (holdResult.RedisUnavailable)
                        throw new ServiceUnavailableException("Tạm thời không thể sử dụng mã giảm giá này, vui lòng thử lại sau.");

                    if (!holdResult.Success)
                        throw new BadRequestException(holdResult.Reason!);
                }

                heldCoupons.Add(coupon.Id);

                var discountAmount = coupon.CalculateDiscountAmount(subtotal, 0, userUsage);

                discounts.Add(new OrderDiscount
                {
                    OrderId = orderId,
                    CouponId = coupon.Id,
                    Code = coupon.Code,
                    DiscountType = coupon.DiscountType,
                    DiscountValue = coupon.DiscountValue,
                    AppliedAmount = discountAmount
                });
            }

            return discounts;
        }

        private static Money CalculateSubtotal(CreateOrderCommand request, List<ProductVariant> variants)
        {
            return request.Items.Aggregate(Money.Zero, (acc, item) =>
            {
                var variant = variants.First(v => v.Id == item.ProductVariantId);
                return acc + (variant.Price * item.Quantity);
            });
        }

        private static ShippingAddress MapShippingAddress(CreateOrderCommand request)
        {
            return new ShippingAddress
            {
                RecipientName = request.ShippingAddress.RecipientName,
                PhoneNumber = request.ShippingAddress.PhoneNumber,
                Email = request.ShippingAddress.Email,
                Province = request.ShippingAddress.Province,
                Ward = request.ShippingAddress.Ward,
                Address = request.ShippingAddress.Address
            };
        }

        private static List<OrderItem> BuildOrderItems(CreateOrderCommand request, List<ProductVariant> variants)
        {
            return request.Items.Select(item =>
            {
                var variant = variants.First(v => v.Id == item.ProductVariantId);
                return new OrderItem
                {
                    ProductVariantId = variant.Id,
                    ProductName = variant.Name,
                    Sku = variant.Sku,
                    UnitPrice = variant.Price,
                    Quantity = item.Quantity,
                    SubTotal = variant.Price * item.Quantity
                };
            }).ToList();
        }

        private void Persist(Order order, Guid customerId)
        {
            dbContext.Orders.Add(order);

            dbContext.OutboxMessages.Add(new OutboxMessage
            {
                Type = OrderCreatedEvent.EventName,
                Payload = JsonSerializer.SerializeToDocument(
                    new OrderCreatedEvent(order.Id, customerId, order.OrderNumber))
            });
        }

        private async Task ReleaseAllHolds(
            Guid orderId, List<Guid> variantIds, List<Guid> heldCouponIds)
        {
            await stockHoldCache.ReleaseAsync(orderId, variantIds);

            foreach (var couponId in heldCouponIds)
            {
                await couponHoldCache.ReleaseAsync(orderId, couponId);
            }
        }
    }
}
