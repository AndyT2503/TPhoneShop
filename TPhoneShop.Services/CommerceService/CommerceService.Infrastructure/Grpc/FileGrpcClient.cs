using CommerceService.Application.Common.Abstractions;
using FileService.Grpc;
using static FileService.Grpc.FileService;

namespace CommerceService.Infrastructure.Grpc
{
    public class FileGrpcClient : IFileGrpcClient
    {
        private readonly FileServiceClient _client;

        public FileGrpcClient(FileServiceClient client)
        {
            _client = client;
        }

        public async Task<string> GetPresignedUrlAsync(Guid mediaId)
        {
            var response = await _client.GetPresignedUrlAsync(
                new GetPresignedUrlRequest
                {
                    MediaId = mediaId.ToString()
                });

            return response.PresignedUrl;
        }
    }
}
