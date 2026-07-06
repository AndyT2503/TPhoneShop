using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CommerceService.Infrastructure.Caching
{
    internal class UserRoleCache(IDistributedCache cache, ILogger<UserRoleCache> logger) : IUserRoleCache
    {
        public async Task<Guid?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try {
                var roleId = await cache.GetStringAsync(CacheKeys.UserRoles(userId), cancellationToken);

                return roleId is null
                    ? null
                    : Guid.Parse(roleId);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to read user role from cache.");
                return null;
            }
        }

        public async Task SetAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        {
            try {
                await cache.SetStringAsync(
                    CacheKeys.UserRoles(userId),
                    roleId.ToString(),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromHours(12)
                    },
                    cancellationToken);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to write user role to cache.");
            }
        }

        public async Task RemoveAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try {
                await cache.RemoveAsync(CacheKeys.UserRoles(userId), cancellationToken);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to remove user role from cache.");
            }
        }
    }
}
