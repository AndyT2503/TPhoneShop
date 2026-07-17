namespace CommerceService.Application.Catalog.Categories.Queries.Dtos
{
    public class CategoryForAdminDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
    }
}
