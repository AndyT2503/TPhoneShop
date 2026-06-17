using IdentityService.Application.Auth.Services;
using IdentityService.Application.Common.Abstractions;
using IdentityService.Infrastructure.BackgroundJobs;
using IdentityService.Infrastructure.Messaging.RabbitMQ;
using IdentityService.Infrastructure.Securities;
using IdentityService.Infrastructure.Securities.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddSingleton<RsaKeyProvider>();
            services.AddScoped<IJwksService, JwksService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IClientInfoService, ClientInfoService>();
            services.AddHostedService<KeyRotationService>();
            services.AddRabbitMq(configuration);
            return services;
        }

        private static IServiceCollection AddRabbitMq(
                this IServiceCollection services,
                IConfiguration config)
        {
            var settings = config.GetSection("RabbitMQ").Get<RabbitMqSettings>()!;
            services.AddScoped<EventDispatcher>();
            services.AddSingleton(settings);
            services.AddSingleton<RabbitMqConnection>();
            services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
            services.AddHostedService<OutboxProcessorService>();
            return services;
        }
    }
}
