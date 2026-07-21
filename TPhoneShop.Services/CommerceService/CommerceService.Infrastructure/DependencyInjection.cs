using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Authorization;
using CommerceService.Infrastructure.BackgroundJobs;
using CommerceService.Infrastructure.Caching;
using CommerceService.Infrastructure.Grpc;
using CommerceService.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
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
            services.AddSingleton<IShippingFeeCalculator, ShippingFeeCalculator>();
            services.AddSingleton<IOrderNumberGenerator, OrderNumberGenerator>();
            return services;
        }

        private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["Redis:ConnectionString"]!;

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });

            // Register IConnectionMultiplexer for Lua script support (stock/coupon holds)
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString));

            services.AddScoped<IMediaCache, MediaCache>();
            services.AddScoped<IRolePermissionCache, RolePermissionCache>();
            services.AddScoped<IUserRoleCache, UserRoleCache>();
            services.AddScoped<IStockHoldCache, StockHoldCache>();
            services.AddScoped<IStockCache, StockCache>();
            services.AddScoped<ICouponHoldCache, CouponHoldCache>();
            services.AddScoped<ICouponUsageCache, CouponUsageCache>();
            services.AddScoped<IIdempotencyCache, IdempotencyCache>();
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
