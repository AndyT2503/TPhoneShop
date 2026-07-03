namespace CommerceService.Application.Catalog.Brands.Queries.Dtos
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Description { get; set; }
        public required string LogoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
