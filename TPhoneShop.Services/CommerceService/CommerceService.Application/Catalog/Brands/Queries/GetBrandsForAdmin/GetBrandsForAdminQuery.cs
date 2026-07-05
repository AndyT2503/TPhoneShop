using BuildingBlocks.Application.Pagination;
using CommerceService.Application.Catalog.Brands.Queries.Dtos;

namespace CommerceService.Application.Catalog.Brands.Queries.GetBrandsForAdmin
{
    public class GetBrandsForAdminQuery : PagingQuery, IRequest<PagingResponse<BrandDto>>
    {
        public string? Search { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
