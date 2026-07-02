using FileService.Application.Common.Dtos;

namespace FileService.Application.Common.Abstractions
{
    public interface IFileStorage
    {
        Task<FileUploadResult> UploadAsync(Stream stream, string originalFileName, string? customFileName = null, CancellationToken cancellationToken = default);

        Task<string> GetPresignedUrl(string fileKey, CancellationToken cancellationToken = default);

        Task DeleteAsync(string fileKey, CancellationToken cancellationToken = default);
    }
}
