namespace CommerceService.Domain.Entities
{
    public class ProductGroup : BaseEntity
    {
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}
