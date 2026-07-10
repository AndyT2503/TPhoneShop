using BuildingBlocks.Infrastructure.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<SoftDeleteInterceptor>();
            services.AddSingleton<AuditableEntityInterceptor>();
            services.AddDbContext<CommerceDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("Commerce"));
                options.AddInterceptors(
                        sp.GetRequiredService<SoftDeleteInterceptor>(),
                        sp.GetRequiredService<AuditableEntityInterceptor>()
                );
            });

            return services;
        }
    }
}
