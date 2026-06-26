namespace CommerceService.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid? ParentId { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; } = [];

        public ICollection<Product> Products { get; set; } = [];

    }
}
