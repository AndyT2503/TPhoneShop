using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Infrastructure.Services
{
    internal class MediaService(IFileGrpcClient fileGrpcClient, IMediaCache mediaCache) : IMediaService
    {
        public async Task<string> GetPresignedUrl(Guid mediaId, CancellationToken cancellationToken)
        {
            var url = await mediaCache.GetPresignedUrlAsync(mediaId, cancellationToken);

            if (!string.IsNullOrWhiteSpace(url))
                return url;

            url = await fileGrpcClient.GetPresignedUrlAsync(mediaId);

            await mediaCache.SetPresignedUrlAsync(
                mediaId,
                url,
                cancellationToken);

            return url;
        }
    }
}
