using CommerceService.Application.Catalog.Brands.Commands.Dtos;

namespace CommerceService.Application.Catalog.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommand : UpdateBrandRequest, IRequest
    {
        public Guid Id { get; set; }
    }
}
