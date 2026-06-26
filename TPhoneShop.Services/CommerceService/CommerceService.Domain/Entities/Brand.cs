namespace CommerceService.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string LogoUrl { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}
