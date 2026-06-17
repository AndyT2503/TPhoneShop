using BuildingBlocks.Infrastructure.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<AuditableEntityInterceptor>();
            services.AddDbContext<MainDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("Auth"));
                options.AddInterceptors(
                        sp.GetRequiredService<AuditableEntityInterceptor>());
            });

            return services;
        }
    }
}
