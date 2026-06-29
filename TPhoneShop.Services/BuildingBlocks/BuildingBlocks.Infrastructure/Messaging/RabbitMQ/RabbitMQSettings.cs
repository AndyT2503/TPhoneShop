namespace BuildingBlocks.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQSettings
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string Exchange { get; set; } = String.Empty;
    }
}
