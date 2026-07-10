namespace CommerceService.Domain.Entities
{
    public class Brand : BaseEntity, ISoftDeletable
    {
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public Guid LogoId { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; } = [];
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
