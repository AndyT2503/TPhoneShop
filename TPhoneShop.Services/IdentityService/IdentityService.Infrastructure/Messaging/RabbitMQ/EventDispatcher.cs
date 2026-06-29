using BuildingBlocks.Application.Messaging;
using IdentityService.Application.Common.Constants;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Messaging.RabbitMQ
{
    public class EventDispatcher
    {
        private readonly IMessageBus _messageBus;
        private static readonly Dictionary<string, string> OutboxRoutingMap = new()
        {
            { OutboxEventTypes.ForgotPassword, RoutingKeys.IdentityUserForgotPassword },
        };

        public EventDispatcher(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task DispatchAsync(OutboxMessage message)
        {
            if (!OutboxRoutingMap.TryGetValue(message.Type, out var routingKey))
            {
                throw new InvalidOperationException($"Unknown event type: {message.Type}");
            }

            await _messageBus.PublishAsync(message.Payload, routingKey);
        }
    }
}
