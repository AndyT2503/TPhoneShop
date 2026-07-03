using FileService.Application.Common.Abstractions;
using FileService.Application.File.Queries.Dtos;

namespace FileService.Application.File.Queries.GetPresignedUrl
{
    internal class GetPresignedUrlQueryHandler(FileDbContext dbContext, IFileStorage fileStorage) : IRequestHandler<GetPresignedUrlQuery, PresignedUrlResponse>
    {
        public async Task<PresignedUrlResponse> Handle(GetPresignedUrlQuery request, CancellationToken cancellationToken)
        {
            var media = await dbContext.Medias.AsNoTracking().FirstOrDefaultAsync(m => m.Id == request.MediaId, cancellationToken);
            if (media is null)
            {
                throw new NotFoundException($"File {request.MediaId} không tồn tại.");
            }

            var presignedUrl = await fileStorage.GetPresignedUrl(media.Key, cancellationToken);
            return new PresignedUrlResponse
            {
                PresignedUrl = presignedUrl
            };
        }
    }
}
