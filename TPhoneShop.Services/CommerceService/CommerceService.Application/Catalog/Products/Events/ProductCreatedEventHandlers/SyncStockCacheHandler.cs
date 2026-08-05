using BuildingBlocks.Application.Events;
using CommerceService.Application.Common.Abstractions;
using CommerceService.Domain.Events.Product;

namespace CommerceService.Application.Catalog.Products.Events.ProductCreatedEventHandlers
{
    internal class SyncStockCacheHandler(
        CommerceDbContext dbContext,
        IStockCache stockCache
    ) : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
    {
        public async Task Handle(DomainEventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var variants = await dbContext.ProductVariants
                .AsNoTracking()
                .Where(v => v.ProductId == notification.Event.ProductId)
                .Select(v => new { v.Id, v.StockQuantity })
                .ToListAsync(cancellationToken);

            var items = variants
                .Select(v => (v.Id, v.StockQuantity))
                .ToList();

            await stockCache.SyncManyAsync(items);
        }
    }
}
