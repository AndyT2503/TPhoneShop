namespace BuildingBlocks.Application.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, string routingKey);
    }
}
