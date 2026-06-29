using BuildingBlocks.Domain.Events;

namespace CommerceService.Domain.Events.Product
{
    public record ProductCreatedEvent(Guid ProductId) : IDomainEvent
    {
        public const string EventName = "product_created_event";
    }
}
