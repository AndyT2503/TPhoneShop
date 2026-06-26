namespace CommerceService.Application.Catalog.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest
    {
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
