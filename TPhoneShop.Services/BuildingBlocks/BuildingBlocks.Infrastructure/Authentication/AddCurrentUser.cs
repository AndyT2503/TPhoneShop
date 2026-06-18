using BuildingBlocks.Application.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Authentication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCurrentUser(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
            return services;
        }
    }
}
