using CommerceService.Persistence;
using CommerceService.ReadModel.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace CommerceService.ReadModel.Catalog.Services.Product
{
    internal class ProductProjectionService(CommerceDbContext dbContext, CommerceMongoDbContext mongoDbContext) : IProductProjectionService
    {
        public async Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            await mongoDbContext.Products.DeleteOneAsync(x => x.Id == productId, cancellationToken);
        }

        public async Task UpsertProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var product = await dbContext.Products
                                         .Include(e => e.Brand)
                                         .Include(e => e.Category)
                                         .Include(e => e.ProductGroup)
                                         .Include(e => e.Variants)
                                         .FirstOrDefaultAsync(e => e.Id == productId, cancellationToken);

            if (product is null)
                return;

            var document = new ProductDocument
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                IsActive = product.IsActive,
                Attributes = product.Attributes
                                    .Select(attr => new ProductAttribute
                                    {
                                        Name = attr.Name,
                                        Value = attr.Value
                                    }).ToList(),
                ProductGroup = product.ProductGroup != null ? new ProductGroup
                {
                    Id = product.ProductGroup.Id,
                    Name = product.ProductGroup.Name
                } : null,
                Brand = new ProductBrand
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                },
                Category = new ProductCategory
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
                Variants = product.Variants.Select(v => new ProductVariant
                {
                    Id = v.Id,
                    Name = v.Name,
                    ThumbnailId = v.ThumbnailId,
                    CompareAtPrice = v.CompareAtPrice,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity,
                    Currency = v.Currency
                }).ToList(),
            };

            await mongoDbContext.Products.ReplaceOneAsync(x => x.Id == document.Id, document, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
    }
}
