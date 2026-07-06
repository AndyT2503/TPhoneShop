using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CommerceService.Infrastructure.Caching
{
    internal class RolePermissionCache(IDistributedCache cache, ILogger<RolePermissionCache> logger) : IRolePermissionCache
    {
        public async Task<HashSet<string>?> GetAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            try {
                var json = await cache.GetStringAsync(CacheKeys.RolePermissions(roleId), cancellationToken);

                return json is null
                    ? null
                    : JsonSerializer.Deserialize<HashSet<string>>(json);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to read role permissions from cache.");
                return null;
            }
        }

        public async Task SetAsync(Guid roleId, HashSet<string> permissions, CancellationToken cancellationToken = default)
        {
            try {
                await cache.SetStringAsync(
                    CacheKeys.RolePermissions(roleId),
                    JsonSerializer.Serialize(permissions),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration =
                            TimeSpan.FromHours(12)
                    },
                    cancellationToken);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to write role permissions to cache.");
            }
        }

        public async Task RemoveAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            try {
                await cache.RemoveAsync(CacheKeys.RolePermissions(roleId), cancellationToken);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to remove role permissions from cache.");
            }
        }
    }
}
