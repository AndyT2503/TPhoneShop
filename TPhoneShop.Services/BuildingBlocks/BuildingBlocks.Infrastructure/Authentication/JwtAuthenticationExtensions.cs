using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.Authentication
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
                        this IServiceCollection services,
                        IConfiguration configuration,
                        IHostEnvironment environment
                )
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.Authority = configuration["Jwt:Issuer"];
                    options.Audience = configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;
                });

            return services;
        }
    }
}
