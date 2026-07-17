namespace CommerceService.Application.Catalog.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest;
}
