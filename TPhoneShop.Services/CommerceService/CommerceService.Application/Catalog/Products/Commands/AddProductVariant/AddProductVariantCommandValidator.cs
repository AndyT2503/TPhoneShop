namespace CommerceService.Application.Catalog.Products.Commands.AddProductVariant
{
    internal class AddProductVariantCommandValidator : AbstractValidator<AddProductVariantCommand>
    {
        public AddProductVariantCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithName("Sản phẩm");

            RuleFor(x => x.Variant.Name)
                .NotEmpty().WithName("Tên biến thể");

            RuleFor(x => x.Variant.Sku)
                .NotEmpty().WithName("Mã SKU");

            RuleFor(x => x.Variant.ThumbnailId)
                .NotEmpty().WithName("Ảnh biến thể");

            RuleFor(x => x.Variant.Price)
                .GreaterThan(0).WithName("Giá tiền");

            RuleFor(x => x.Variant.StockQuantity)
                .GreaterThanOrEqualTo(0).WithName("Số lượng tồn kho");
        }
    }
}
