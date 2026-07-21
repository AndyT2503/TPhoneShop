using BuildingBlocks.Application.Events;
using CommerceService.Application.Common.Abstractions;
using CommerceService.Domain.Constants;
using CommerceService.Domain.Events.Order;

namespace CommerceService.Application.Orders.Events.OrderCreatedEventHandlers
{
    internal class ConfirmOfflinePaymentOrderHandler(
        CommerceDbContext dbContext,
        IStockHoldCache stockHoldCache,
        ICouponHoldCache couponHoldCache,
        IStockCache stockCache,
        ICouponUsageCache couponUsageCache
    ) : INotificationHandler<DomainEventNotification<OrderCreatedEvent>>
    {
        public async Task Handle(DomainEventNotification<OrderCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.Event;

            var order = await dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderDiscounts)
                .FirstOrDefaultAsync(o => o.Id == domainEvent.OrderId, cancellationToken);

            if (order is null || order.Status != OrderStatuses.Pending)
                return;

            if (!PaymentMethods.IsOfflinePayment(order.PaymentMethod))
                return;

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            order.Confirm();
            order.AddLog(OrderLogAction.OrderStatusChange, order.CustomerId);
            await DeductStock(order, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await ReleaseHolds(order);
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

        private async Task ReleaseHolds(Order order)
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
