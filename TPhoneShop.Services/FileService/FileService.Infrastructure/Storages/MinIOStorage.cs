using BuildingBlocks.Application.Slug;
using FileService.Application.Common.Abstractions;
using FileService.Application.Common.Dtos;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace FileService.Infrastructure.Storages
{
    internal class MinIOStorage(IMinioClient client, ISlugGenerator slugGenerator, IConfiguration configuration) : IFileStorage
    {
        private readonly string _bucketName = configuration["Minio:BucketName"]!;
        private readonly int _expiryInSeconds = 60 * 60;
        public async Task DeleteAsync(string fileKey, CancellationToken cancellationToken = default)
        {
            await client.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(_bucketName).WithObject(fileKey), cancellationToken);
        }

        public async Task<string> GetPresignedUrl(string fileKey, CancellationToken cancellationToken = default)
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                                                        .WithBucket(_bucketName)
                                                        .WithObject(fileKey)
                                                        .WithExpiry(_expiryInSeconds);
            return await client.PresignedGetObjectAsync(presignedGetObjectArgs);
        }

        public async Task<FileUploadResult> UploadAsync(Stream stream, string originalFileName, string? customFileName = null, CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(originalFileName);
            var slug = slugGenerator.Generate(customFileName ?? Path.GetFileNameWithoutExtension(originalFileName));
            var random = Random.Shared.Next(100000, 999999);
            var fileKey = $"{slug}-{random}{extension}";

            var putObjectArgs = new PutObjectArgs()
                                        .WithBucket(_bucketName)
                                        .WithObject(fileKey)
                                        .WithStreamData(stream)
                                        .WithObjectSize(stream.Length);
            await client.PutObjectAsync(putObjectArgs, cancellationToken);
            var presignedUrl = await GetPresignedUrl(fileKey);

            return new FileUploadResult(fileKey, presignedUrl);
        }
    }
}
