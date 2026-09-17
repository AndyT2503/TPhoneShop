using BuildingBlocks.Application.Pagination;
using CommerceService.Application.Catalog.Categories.Queries.Dtos;

namespace CommerceService.Application.Catalog.Categories.Queries.GetPublicCategories
{
    public class GetPublicCategoriesQuery : IRequest<IReadOnlyCollection<CategoryDto>>
    {
    }
}
