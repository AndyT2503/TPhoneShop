namespace CommerceService.ReadModel.Catalog.Products
{
    public class ProductDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public ProductBrand Brand { get; set; } = default!;
        public ProductCategory Category { get; set; } = default!;
        public ProductGroup? ProductGroup { get; set; }
        public List<ProductAttribute> Attributes { get; set; } = [];
        public List<ProductVariant> Variants { get; set; } = [];
        public bool IsActive { get; set; }
    }

    public class ProductBrand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }

    public class ProductCategory
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }

    public class ProductAttribute
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

    public class ProductGroup
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }

    public class ProductVariant
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string ThumbnailUrl { get; set; }
        public long Price { get; set; }
        public long? CompareAtPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
