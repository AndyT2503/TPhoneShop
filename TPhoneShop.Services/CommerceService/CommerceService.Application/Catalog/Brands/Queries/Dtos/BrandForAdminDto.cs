namespace CommerceService.Application.Catalog.Brands.Queries.Dtos
{
    public class BrandForAdminDto : BrandDto
    {
        public Guid LogoId { get; set; }
        public bool IsActive { get; set; }
    }
}
