using CommerceService.Application.Common.Abstractions;
using CommerceService.Domain.Constants;

namespace CommerceService.Application.Orders.Commands.ConfirmOrderPayment
{
    internal class ConfirmOrderPaymentCommandHandler(
        CommerceDbContext dbContext,
        IStockHoldCache stockHoldCache,
        IStockCache stockCache,
        ICouponHoldCache couponHoldCache,
        ICouponUsageCache couponUsageCache
    ) : IRequestHandler<ConfirmOrderPaymentCommand>
    {
        public async Task Handle(ConfirmOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var order = await dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderDiscounts)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new NotFoundException("Đơn hàng không tồn tại.");

            if (order.PaymentStatus == PaymentStatuses.Paid
                || order.PaymentStatus == PaymentStatuses.Refunded)
                return;

            if (PaymentMethods.IsOfflinePayment(order.PaymentMethod))
                return;

            if (order.Status == OrderStatuses.Cancelled)
            {
                order.MarkAsRefunded();
                order.AddLog(OrderLogAction.PaymentStatusChange, order.CustomerId);
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return;
            }

            order.MarkAsPaid();
            order.AddLog(OrderLogAction.PaymentStatusChange, order.CustomerId);
            order.Confirm();
            order.AddLog(OrderLogAction.OrderStatusChange, order.CustomerId);

            await DeductStock(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await ReleaseStockHold(order);
        }

        private async Task DeductStock(Order order, CancellationToken cancellationToken)
        {
            foreach (var item in order.OrderItems)
            {
                var variant = await dbContext.ProductVariants
                    .FirstAsync(v => v.Id == item.ProductVariantId, cancellationToken);

                var beforeQty = variant.StockQuantity;
                variant.StockQuantity -= item.Quantity;

                dbContext.InventoryTransactions.Add(new InventoryTransaction
                {
                    ProductVariantId = variant.Id,
                    Action = InventoryActions.OrderPlaced,
                    BeforeQuantity = beforeQty,
                    QuantityChanged = -item.Quantity,
                    AfterQuantity = variant.StockQuantity,
                    OrderId = order.Id,
                    PerformedBy = order.CustomerId
                });
            }
        }

        private async Task ReleaseStockHold(Order order)
        {
            var variantIds = order.OrderItems.Select(i => i.ProductVariantId).ToList();
            await stockHoldCache.ReleaseAsync(order.Id, variantIds);

            foreach (var item in order.OrderItems)
            {
                await stockCache.DecrementAsync(item.ProductVariantId, item.Quantity);
            }

            foreach (var discount in order.OrderDiscounts)
            {
                await couponHoldCache.ReleaseAsync(order.Id, discount.CouponId);
                await couponUsageCache.IncrementUsageAsync(discount.CouponId);
            }
        }
    }
}
