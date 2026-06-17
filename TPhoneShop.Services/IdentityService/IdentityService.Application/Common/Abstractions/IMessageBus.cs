namespace IdentityService.Application.Common.Abstractions
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, string routingKey);
    }
}
