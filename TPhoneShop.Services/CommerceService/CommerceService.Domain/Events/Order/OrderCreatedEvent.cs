using BuildingBlocks.Domain.Events;

namespace CommerceService.Domain.Events.Order
{
    public record OrderCreatedEvent(Guid OrderId, Guid CustomerId, string OrderNumber) : IDomainEvent
    {
        public const string EventName = "order_created_event";
    }
}
