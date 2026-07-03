using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Authorization;
using CommerceService.Infrastructure.BackgroundJobs;
using CommerceService.Infrastructure.Caching;
using CommerceService.Infrastructure.Grpc;
using CommerceService.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static FileService.Grpc.FileService;

namespace CommerceService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddScoped<IMediaService, MediaService>();
            services.AddGrpc(configuration);
            services.AddRedisCache(configuration);
            services.AddHostedService<SyncPermissionService>();
            services.AddScoped<UserAuthorizationService>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }

        private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:ConnectionString"];
            });

            services.AddScoped<IMediaCache, MediaCache>();
            services.AddScoped<IRolePermissionCache, RolePermissionCache>();
            services.AddScoped<IUserRoleCache, UserRoleCache>();
            return services;
        }

        private static IServiceCollection AddGrpc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<FileServiceClient>(options =>
            {
                options.Address = new Uri(configuration["Grpc:FileServiceUrl"]!);
            });
            services.AddScoped<IFileGrpcClient, FileGrpcClient>();
            return services;
        }
    }
}
