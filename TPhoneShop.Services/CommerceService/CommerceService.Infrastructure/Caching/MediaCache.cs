using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CommerceService.Infrastructure.Caching
{
    internal class MediaCache(IDistributedCache cache, ILogger<MediaCache> logger) : IMediaCache
    {
        private static string GetKey(Guid mediaId) => $"media:presigned-url:{mediaId}";
        public async Task<string?> GetPresignedUrlAsync(Guid mediaId, CancellationToken cancellationToken = default)
        {
            try {
                return await cache.GetStringAsync(GetKey(mediaId), cancellationToken);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to read media presigned url from cache.");
                return null;
            }
        }

        public async Task SetPresignedUrlAsync(Guid mediaId, string presignedUrl, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            try {
                await cache.SetStringAsync(GetKey(mediaId), presignedUrl, options, cancellationToken);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to write media presigned url to cache.");
            }
        }
    }
}
