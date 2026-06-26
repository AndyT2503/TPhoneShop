using CommerceService.ReadModel.Catalog.Services.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CommerceService.ReadModel
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddReadModel(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                return new MongoClient(
                    configuration.GetConnectionString("MongoProjection"));
            });

            services.AddSingleton<CommerceMongoDbContext>();
            services.AddScoped<IProductProjectionService, ProductProjectionService>();
            return services;
        }
    }
}
