namespace CommerceService.Application.Catalog.Brands.Commands.Dtos
{
    public class UpdateBrandRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid LogoId { get; set; }
        public bool IsActive { get; set; }
    }
}
