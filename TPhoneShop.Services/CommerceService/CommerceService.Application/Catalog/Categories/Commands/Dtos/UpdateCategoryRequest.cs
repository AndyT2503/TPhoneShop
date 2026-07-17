namespace CommerceService.Application.Catalog.Categories.Commands.Dtos
{
    public class UpdateCategoryRequest
    {
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
