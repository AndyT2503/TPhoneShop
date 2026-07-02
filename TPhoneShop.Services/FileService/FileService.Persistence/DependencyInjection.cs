using BuildingBlocks.Infrastructure.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AuditableEntityInterceptor>();
            services.AddDbContext<FileDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("File"));
                options.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
            });

            return services;
        }
    }
}
