using CommerceService.Application.Catalog.Products.Commands.Dtos;

namespace CommerceService.Application.Catalog.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest
    {
        public Guid? ProductGroupId { get; set; }
        public Guid CategoryId { get; set; }

        public Guid BrandId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public List<ProductAttributeRequest> Attributes { get; set; } = [];

        public List<CreateProductVariantRequest> Variants { get; set; } = [];
    }
}
