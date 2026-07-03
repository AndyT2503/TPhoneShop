using FileService.Application.File.Queries.GetPresignedUrl;
using FileService.Grpc;
using Grpc.Core;
using MediatR;
using static FileService.Grpc.FileService;

namespace FileService.API.Grpc
{
    public class FileGrpcService(IMediator mediator) : FileServiceBase
    {
        public override async Task<GetPresignedUrlResponse> GetPresignedUrl(GetPresignedUrlRequest request, ServerCallContext context)
        {
            var result = await mediator.Send(new GetPresignedUrlQuery(Guid.Parse(request.MediaId)));
            return new GetPresignedUrlResponse
            {
                PresignedUrl = result.PresignedUrl
            };
        }
    }
}
