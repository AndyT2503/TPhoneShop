namespace CommerceService.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Guid? ProductGroupId { get; set; }
        public Guid CategoryId { get; set; }

        public Guid BrandId { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public Category Category { get; set; } = null!;

        public Brand Brand { get; set; } = null!;
        /// <summary>
        /// Postgres SQL JsonB
        /// </summary>
        public List<ProductAttribute> Attributes { get; set; } = [];

        public ICollection<ProductVariant> Variants { get; set; } = [];
        public ProductGroup? ProductGroup { get; set; }

    }
}
