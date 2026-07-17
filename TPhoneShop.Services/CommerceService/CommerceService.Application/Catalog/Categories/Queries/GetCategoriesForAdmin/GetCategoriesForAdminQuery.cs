using CommerceService.Application.Catalog.Categories.Queries.Dtos;

namespace CommerceService.Application.Catalog.Categories.Queries.GetCategoriesForAdmin
{
    public class GetCategoriesForAdminQuery : IRequest<List<CategoryForAdminDto>>
    {
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
