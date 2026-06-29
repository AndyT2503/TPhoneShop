using BuildingBlocks.Application.Messaging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQMessageBus : IMessageBus, IAsyncDisposable
    {
        private readonly RabbitMQConnection _connection;
        private readonly RabbitMQSettings _settings;

        private IChannel? _channel;

        public RabbitMQMessageBus(
            RabbitMQConnection connection,
            RabbitMQSettings settings)
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
