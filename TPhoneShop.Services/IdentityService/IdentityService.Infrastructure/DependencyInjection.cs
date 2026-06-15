using IdentityService.Application.Auth.Services;
using IdentityService.Infrastructure.BackgroundJobs;
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
            services.AddScoped<IClientInfoService, ClientInfoService>();
            services.AddHostedService<KeyRotationHostedService>();
            return services;
        }
    }
}
