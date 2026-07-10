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
            services.AddScoped<SoftDeleteInterceptor>();
            services.AddSingleton<AuditableEntityInterceptor>();
            services.AddDbContext<IdentityDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("Auth"));
                options.AddInterceptors(
                        sp.GetRequiredService<SoftDeleteInterceptor>(),
                        sp.GetRequiredService<AuditableEntityInterceptor>()
                );
            });

            return services;
        }
    }
}
