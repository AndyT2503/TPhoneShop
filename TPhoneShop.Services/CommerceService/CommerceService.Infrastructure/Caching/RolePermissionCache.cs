using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Constants;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CommerceService.Infrastructure.Caching
{
    public class RolePermissionCache : IRolePermissionCache
    {
        private readonly IDistributedCache _cache;

        public RolePermissionCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<HashSet<string>?> GetAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            var json = await _cache.GetStringAsync(CacheKeys.RolePermissions(roleId), cancellationToken);

            return json is null
                ? null
                : JsonSerializer.Deserialize<HashSet<string>>(json);
        }

        public async Task SetAsync(Guid roleId, HashSet<string> permissions, CancellationToken cancellationToken = default)
        {
            await _cache.SetStringAsync(
                CacheKeys.RolePermissions(roleId),
                JsonSerializer.Serialize(permissions),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration =
                        TimeSpan.FromHours(12)
                },
                cancellationToken);
        }

        public async Task RemoveAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(CacheKeys.RolePermissions(roleId), cancellationToken);
        }
    }
}
