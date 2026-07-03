using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace CommerceService.Infrastructure.Caching
{
    internal class MediaCache(IDistributedCache distributedCache) : IMediaCache
    {
        private static string GetKey(Guid mediaId) => $"media:presigned-url:{mediaId}";
        public async Task<string?> GetPresignedUrlAsync(Guid mediaId, CancellationToken cancellationToken = default)
        {
            return await distributedCache.GetStringAsync(GetKey(mediaId), cancellationToken);
        }

        public async Task SetPresignedUrlAsync(Guid mediaId, string presignedUrl, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            await distributedCache.SetStringAsync(GetKey(mediaId), presignedUrl, options, cancellationToken);
        }
    }
}
