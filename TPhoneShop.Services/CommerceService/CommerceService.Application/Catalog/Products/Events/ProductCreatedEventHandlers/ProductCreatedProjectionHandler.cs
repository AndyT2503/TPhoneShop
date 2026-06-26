using CommerceService.Application.Common.Events;
using CommerceService.Domain.Events.Product;
using CommerceService.ReadModel.Catalog.Services.Product;

namespace CommerceService.Application.Catalog.Products.Events.ProductCreatedEventHandlers
{
    internal class ProductCreatedProjectionHandler(IProductProjectionService productProjectionService) : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
    {
        public async Task Handle(DomainEventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
        {
            await productProjectionService.UpsertProductAsync(notification.Event.ProductId, cancellationToken);
        }
    }
}
