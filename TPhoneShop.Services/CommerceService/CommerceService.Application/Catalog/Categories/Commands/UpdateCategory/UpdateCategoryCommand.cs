namespace CommerceService.Application.Catalog.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
