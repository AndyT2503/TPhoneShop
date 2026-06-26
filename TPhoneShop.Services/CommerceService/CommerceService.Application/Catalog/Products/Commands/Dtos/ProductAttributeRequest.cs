namespace CommerceService.Application.Catalog.Products.Commands.Dtos
{
    public class ProductAttributeRequest
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }
}
