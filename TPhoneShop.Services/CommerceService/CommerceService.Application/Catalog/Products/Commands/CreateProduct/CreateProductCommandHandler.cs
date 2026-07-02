using BuildingBlocks.Application.Slug;
using CommerceService.Domain.Events.Product;
using System.Text.Json;

namespace CommerceService.Application.Catalog.Products.Commands.CreateProduct
{
    internal class CreateProductCommandHandler(CommerceDbContext dbContext, ISlugGenerator slugGenerator) : IRequestHandler<CreateProductCommand>
    {
        public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await dbContext.Categories
                                                .AnyAsync(e => e.Id == request.CategoryId && e.IsActive, cancellationToken);

            if (!categoryExists)
            {
                throw new NotFoundException("Danh mục không tồn tại.");
            }

            var brandExists = await dbContext.Brands
                                             .AnyAsync(e => e.Id == request.BrandId && e.IsActive, cancellationToken);

            if (!brandExists)
            {
                throw new NotFoundException("Thương hiệu không tồn tại.");
            }

            if (request.ProductGroupId is not null)
            {
                var productGroupExists = await dbContext.ProductGroups.AnyAsync(e => e.Id == request.ProductGroupId && e.IsActive, cancellationToken);
                if (!productGroupExists)
                {
                    throw new NotFoundException("Nhóm sản phẩm không tồn tại.");
                }
            }



            var duplicateSkuInRequest = request.Variants
                .GroupBy(e => e.Sku.Trim().ToUpperInvariant())
                .Any(x => x.Count() > 1);

            if (duplicateSkuInRequest)
            {
                throw new BadRequestException("SKU bị trùng trong danh sách biến thể.");
            }

            var skus = request.Variants
                .Select(x => x.Sku.Trim())
                .ToList();

            if (skus.Count > 0)
            {
                var duplicatedSkuInDatabase = await dbContext.ProductVariants
                                                             .AnyAsync(x => skus.Contains(x.Sku), cancellationToken);

                if (duplicatedSkuInDatabase)
                {
                    throw new BadRequestException("SKU đã tồn tại.");
                }
            }

            var product = new Product
            {
                ProductGroupId = request.ProductGroupId,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
                Name = request.Name.Trim(),
                Slug = slugGenerator.Generate(request.Name),
                Description = request.Description,
                IsActive = true,
                Attributes = request.Attributes
                    .Select(x => new ProductAttribute
                    {
                        Name = x.Name,
                        Value = x.Value
                    })
                    .ToList()
            };

            foreach (var variantRequest in request.Variants)
            {
                if (variantRequest.Price <= 0)
                {
                    throw new BadRequestException($"Giá bán của biến thể '{variantRequest.Name}' phải lớn hơn 0.");
                }

                if (variantRequest.CompareAtPrice.HasValue &&
                    variantRequest.CompareAtPrice.Value < variantRequest.Price)
                {
                    throw new BadRequestException($"Giá niêm yết của biến thể '{variantRequest.Name}' phải lớn hơn hoặc bằng giá bán.");
                }

                if (variantRequest.StockQuantity < 0)
                {
                    throw new BadRequestException($"Tồn kho của biến thể '{variantRequest.Name}' không hợp lệ.");
                }

                product.Variants.Add(new ProductVariant
                {
                    Name = variantRequest.Name.Trim(),
                    Sku = variantRequest.Sku.Trim(),
                    ThumbnailUrl = variantRequest.ThumbnailUrl,
                    Price = variantRequest.Price * 100,
                    CompareAtPrice = variantRequest.CompareAtPrice.HasValue
                        ? variantRequest.CompareAtPrice.Value * 100
                        : null,
                    StockQuantity = variantRequest.StockQuantity,
                    IsActive = true
                });
            }

            dbContext.Products.Add(product);
            dbContext.OutboxMessages.Add(new OutboxMessage
            {
                Type = ProductCreatedEvent.EventName,
                Payload = JsonSerializer.SerializeToDocument(new ProductCreatedEvent(product.Id))
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
