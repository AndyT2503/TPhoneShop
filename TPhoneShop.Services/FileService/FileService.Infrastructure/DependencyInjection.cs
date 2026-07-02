using FileService.Application.Common.Abstractions;
using FileService.Infrastructure.Storages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Args;

namespace FileService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFileStorage(configuration);
            return services;
        }

        private static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileStorage, MinIOStorage>();
            services.AddSingleton<IMinioClient>(sp =>
            {
                var endpoint = configuration["Minio:Endpoint"]!;
                var accessKey = configuration["Minio:AccessKey"]!;
                var secretKey = configuration["Minio:SecretKey"]!;
                var bucket = configuration["Minio:BucketName"]!;
                var useSsl = configuration.GetValue<bool>("Minio:UseSsl");
                var builder = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey);

                if (useSsl)
                {
                    builder = builder.WithSSL();
                }

                var client = builder.Build();

                var exists = client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket)).GetAwaiter().GetResult();
                if (!exists)
                {
                    client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket)).GetAwaiter().GetResult();
                }

                return client;
            });
            return services;
        }
    }
}
