namespace CommerceService.Application.Catalog.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid LogoId { get; set; }
    }
}
