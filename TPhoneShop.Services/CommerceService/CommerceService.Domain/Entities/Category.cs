namespace CommerceService.Domain.Entities
{
    public class Category : BaseEntity, ISoftDeletable
    {
        public Guid? ParentId { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; } = [];

        public ICollection<Product> Products { get; set; } = [];
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

    }
}
