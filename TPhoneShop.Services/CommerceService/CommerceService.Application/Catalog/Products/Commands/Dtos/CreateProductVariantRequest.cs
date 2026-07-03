namespace CommerceService.Application.Catalog.Products.Commands.Dtos
{
    public class CreateProductVariantRequest
    {
        public required string Name { get; set; }

        public required string Sku { get; set; }

        public Guid ThumbnailId { get; set; }
        public List<ProductAttributeRequest> Attributes { get; set; } = [];
        /// <summary>
        /// Selling price stored as amount ×100.
        /// Example: 29,990,000 VND => 2,999,000,000.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Original/list price stored as amount ×100.
        /// Null if no comparison price is available.
        /// Example: 32,990,000 VND => 3,299,000,000.
        /// </summary>
        public long? CompareAtPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
