using CommerceService.Application.Catalog.Categories.Queries.Dtos;

namespace CommerceService.Application.Catalog.Categories.Queries.GetPublicCategories
{
    public class GetPublicCategoriesQuery : IRequest<List<CategoryDto>>
    {
        public string? Search { get; set; }
    }
}
