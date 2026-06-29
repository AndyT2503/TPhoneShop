using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using IdentityService.Application.Auth.Services;
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
                IConfiguration configuration
        )
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddSingleton<RsaKeyProvider>();
            services.AddScoped<IJwksService, JwksService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IClientInfoService, ClientInfoService>();
            services.AddHostedService<KeyRotationService>();
            services.AddFirebase();
            services.AddScoped<EventDispatcher>();
            services.AddHostedService<OutboxProcessorService>();
            return services;
        }

        private static IServiceCollection AddFirebase(this IServiceCollection services)
        {
            if (FirebaseApp.DefaultInstance != null)
            {
                return services;
            }

            var assembly = typeof(DependencyInjection).Assembly;

            using var stream = assembly.GetManifestResourceStream("IdentityService.Infrastructure.Securities.Firebase.tphoneshop-firebase.json");

            if (stream is null)
            {
                throw new InvalidOperationException("Firebase service account not found.");
            }

            var credential = ServiceAccountCredential.FromServiceAccountData(stream);
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential.ToGoogleCredential()
            });

            return services;
        }
    }
}
