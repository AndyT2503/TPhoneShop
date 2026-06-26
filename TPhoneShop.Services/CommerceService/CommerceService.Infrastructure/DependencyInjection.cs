using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Authorization;
using CommerceService.Infrastructure.BackgroundJobs;
using CommerceService.Infrastructure.Caching;
using CommerceService.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:ConnectionString"];
            });
            services.AddScoped<IRolePermissionCache, RolePermissionCache>();
            services.AddScoped<IUserRoleCache, UserRoleCache>();
            services.AddSingleton<ISlugGenerator, SlugGenerator>();
            services.AddHostedService<SyncPermissionService>();
            services.AddScoped<UserAuthorizationService>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }
    }
}
