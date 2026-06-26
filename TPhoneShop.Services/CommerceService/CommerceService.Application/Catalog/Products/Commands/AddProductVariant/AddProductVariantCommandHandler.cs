namespace CommerceService.Application.Catalog.Products.Commands.AddProductVariant
{
    internal class AddProductVariantCommandHandler(CommerceDbContext dbContext) : IRequestHandler<AddProductVariantCommand>
    {
        public async Task Handle(AddProductVariantCommand request, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(e => e.Id == request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Sản phẩm không tồn tại.");
            }

            if (!product.IsActive)
            {
                throw new BadRequestException("Không thể thêm biến thể cho sản phẩm đã ngừng kinh doanh.");
            }

            var skuExists = await dbContext.ProductVariants.AnyAsync(e => e.Sku == request.Variant.Sku, cancellationToken);

            if (skuExists)
            {
                throw new BadRequestException("SKU đã tồn tại trong hệ thống.");
            }

            if (request.Variant.CompareAtPrice.HasValue &&
                request.Variant.CompareAtPrice.Value <= request.Variant.Price)
            {
                throw new BadRequestException("Giá niêm yết phải lớn hơn giá bán.");
            }

            var variant = new ProductVariant
            {
                ProductId = request.ProductId,
                Name = request.Variant.Name,
                Sku = request.Variant.Sku,
                ThumbnailUrl = request.Variant.ThumbnailUrl,
                Price = request.Variant.Price * 100,
                CompareAtPrice = request.Variant.CompareAtPrice * 100,
                StockQuantity = request.Variant.StockQuantity,
                IsActive = true,
            };

            dbContext.ProductVariants.Add(variant);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
