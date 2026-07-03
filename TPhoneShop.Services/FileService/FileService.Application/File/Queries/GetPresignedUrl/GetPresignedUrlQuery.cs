using FileService.Application.File.Queries.Dtos;

namespace FileService.Application.File.Queries.GetPresignedUrl
{
    public record GetPresignedUrlQuery(Guid MediaId) : IRequest<PresignedUrlResponse>;
}
