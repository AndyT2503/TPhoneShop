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
            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("Auth"));
            });

            return services;
        }
    }
}
