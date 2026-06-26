using CommerceService.Application.Catalog.Products.Commands.Dtos;

namespace CommerceService.Application.Catalog.Products.Commands.AddProductVariant
{
    public class AddProductVariantCommand : IRequest
    {
        public Guid ProductId { get; set; }

        public required CreateProductVariantRequest Variant { get; set; }
    }
}
