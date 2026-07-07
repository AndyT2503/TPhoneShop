using FileService.Application.Common.Abstractions;
using FileService.Application.File.Commands.Dtos;

namespace FileService.Application.File.Commands.UploadFile
{
    internal class UploadFileCommandHandler(FileDbContext dbContext, IFileStorage fileStorage) : IRequestHandler<UploadFileCommand, UploadFileResponse>
    {
        public async Task<UploadFileResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var file = request.File;
            var originalFileName = file.FileName;
            var stream = file.OpenReadStream();
            var contentType = file.ContentType;
            var size = file.Length;

            var uploadResult = await fileStorage.UploadAsync(stream, originalFileName, request.CustomFileName, cancellationToken);
            var newMedia = new Media
            {
                Key = uploadResult.FileKey,
                ContentType = contentType,
                Size = size,
            };
            dbContext.Medias.Add(newMedia);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UploadFileResponse
            {
                MediaId = newMedia.Id,
                PresignedUrl = uploadResult.Url,
            };
        }
    }
}
