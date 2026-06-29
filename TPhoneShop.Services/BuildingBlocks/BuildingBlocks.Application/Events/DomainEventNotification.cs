using BuildingBlocks.Domain.Events;
using MediatR;

namespace BuildingBlocks.Application.Events
{
    public record DomainEventNotification<TEvent>(TEvent Event) : INotification where TEvent : IDomainEvent;
}
