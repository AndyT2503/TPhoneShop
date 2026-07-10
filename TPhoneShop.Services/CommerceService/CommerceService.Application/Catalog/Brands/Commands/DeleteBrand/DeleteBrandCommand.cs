namespace CommerceService.Application.Catalog.Brands.Commands.DeleteBrand
{
    public record DeleteBrandCommand(Guid Id) : IRequest;
}
