using RabbitMQ.Client;

namespace BuildingBlocks.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQConnection : IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;

        public RabbitMQConnection(RabbitMQSettings settings)
        {
            _factory = new ConnectionFactory
            {
                HostName = settings.Host,
                Port = settings.Port,
                UserName = settings.Username,
                Password = settings.Password
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection is { IsOpen: true })
                return _connection;

            _connection = await _factory.CreateConnectionAsync();
            return _connection;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}
