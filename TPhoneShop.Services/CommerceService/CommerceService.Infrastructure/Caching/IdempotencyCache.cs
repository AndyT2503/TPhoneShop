using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CommerceService.Infrastructure.Caching
{
    internal class IdempotencyCache(IDistributedCache cache, ILogger<IdempotencyCache> logger) : IIdempotencyCache
    {
        private static string GetKey(string idempotencyKey) => $"idempotency:{idempotencyKey}";

        public async Task<T?> GetExistingResultAsync<T>(string key) where T : class
        {
            try
            {
                var cached = await cache.GetStringAsync(GetKey(key));
                if (cached is null) return null;
                return JsonSerializer.Deserialize<T>(cached);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to read idempotency key {Key} from cache.", key);
                return null;
            }
        }

        public async Task SaveResultAsync<T>(string key, T result, TimeSpan expiration) where T : class
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                };

                var serialized = JsonSerializer.Serialize(result);
                await cache.SetStringAsync(GetKey(key), serialized, options);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save idempotency key {Key} to cache.", key);
            }
        }
    }
}
