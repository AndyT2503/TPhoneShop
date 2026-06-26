using CommerceService.ReadModel.Catalog.Products;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CommerceService.ReadModel
{
    public sealed class CommerceMongoDbContext(IMongoClient mongoClient, IConfiguration configuration)
    {
        private readonly IMongoDatabase _database = mongoClient.GetDatabase(configuration["MongoDb:DatabaseName"]
                                                                            ?? throw new InvalidOperationException("MongoDb:DatabaseName is not configured."));

        public IMongoCollection<ProductDocument> Products => _database.GetCollection<ProductDocument>("products");
    }
}
