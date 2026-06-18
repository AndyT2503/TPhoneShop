using CommerceService.Application.Common.Abstractions;
using CommerceService.Infrastructure.Constants;
using Microsoft.Extensions.Caching.Distributed;

namespace CommerceService.Infrastructure.Caching
{
    public class UserRoleCache : IUserRoleCache
    {
        private readonly IDistributedCache _cache;

        public UserRoleCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<Guid?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var roleId = await _cache.GetStringAsync(CacheKeys.UserRoles(userId), cancellationToken);

            return roleId is null
                ? null
                : Guid.Parse(roleId);
        }

        public async Task SetAsync(Guid userId, Guid roledId, CancellationToken cancellationToken = default)
        {
            await _cache.SetStringAsync(
                CacheKeys.UserRoles(userId),
                roledId.ToString(),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(12)
                },
                cancellationToken);
        }

        public async Task RemoveAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(CacheKeys.UserRoles(userId), cancellationToken);
        }
    }
}
