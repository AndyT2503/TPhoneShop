using BuildingBlocks.Application.Pagination;
using CommerceService.Application.Catalog.Brands.Queries.Dtos;

namespace CommerceService.Application.Catalog.Brands.Queries.GetPublicBrands
{
    public class GetPublicBrandsQuery : PagingQuery, IRequest<PagingResponse<BrandDto>>
    {
    }
}
