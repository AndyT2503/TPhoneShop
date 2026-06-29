using BuildingBlocks.Application.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Messaging.RabbitMQ
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration config, string settingsSection)
        {
            var settings = config.GetSection(settingsSection).Get<RabbitMQSettings>()!;
            services.AddSingleton(settings);
            services.AddSingleton<RabbitMQConnection>();
            services.AddSingleton<IMessageBus, RabbitMQMessageBus>();
            return services;
        }
    }
}
