namespace CommerceService.Application.Common.Abstractions
{
    public interface IMediaCache
    {
        Task<string?> GetPresignedUrlAsync(Guid mediaId, CancellationToken cancellationToken = default);
        Task SetPresignedUrlAsync(Guid mediaId, string presignedUrl, CancellationToken cancellationToken = default);
    }
}
