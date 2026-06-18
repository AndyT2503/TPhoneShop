using IdentityService.Application.Common.Abstractions;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace IdentityService.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqMessageBus : IMessageBus, IAsyncDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly RabbitMqSettings _settings;

        private IChannel? _channel;

        public RabbitMqMessageBus(
            RabbitMqConnection connection,
            RabbitMqSettings settings)
        {
            _connection = connection;
            _settings = settings;
        }

        public async Task PublishAsync<T>(
            T message,
            string routingKey)
        {
            var channel = await GetChannelAsync();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: _settings.Exchange,
                routingKey: routingKey,
                body: body
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.DisposeAsync();
        }

        private async Task<IChannel> GetChannelAsync()
        {
            if (_channel is { IsOpen: true })
                return _channel;

            var conn = await _connection.GetConnectionAsync();

            _channel = await conn.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                exchange: _settings.Exchange,
                type: ExchangeType.Direct,
                durable: true
            );

            return _channel;
        }
    }
}
