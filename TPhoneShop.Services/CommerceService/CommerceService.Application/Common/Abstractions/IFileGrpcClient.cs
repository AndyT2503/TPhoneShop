namespace CommerceService.Application.Common.Abstractions
{
    public interface IFileGrpcClient
    {
        Task<string> GetPresignedUrlAsync(Guid mediaId);
    }
}
