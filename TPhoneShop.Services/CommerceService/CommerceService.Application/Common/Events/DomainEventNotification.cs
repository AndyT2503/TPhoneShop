using CommerceService.Domain.Events.Abstractions;

namespace CommerceService.Application.Common.Events
{
    public record DomainEventNotification<TEvent>(TEvent Event) : INotification where TEvent : IDomainEvent;
}
