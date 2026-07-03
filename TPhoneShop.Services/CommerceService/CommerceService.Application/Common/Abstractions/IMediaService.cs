namespace CommerceService.Application.Common.Abstractions
{
    public interface IMediaService
    {
        Task<string> GetPresignedUrl(Guid mediaId, CancellationToken cancellationToken);
    }
}
