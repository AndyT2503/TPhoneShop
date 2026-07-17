using BuildingBlocks.Infrastructure.Extensions;
using CommerceService.Application.Catalog.Categories.Queries.Dtos;
using CommerceService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommerceService.Application.Catalog.Categories.Queries.GetCategoriesForAdmin
{
    public class GetCategoriesForAdminQueryHandler(CommerceDbContext dbContext) : IRequestHandler<GetCategoriesForAdminQuery, List<CategoryForAdminDto>>
    {
        public async Task<List<CategoryForAdminDto>> Handle(GetCategoriesForAdminQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Categories
                                 .AsNoTracking()
                                 .WhereIf(!string.IsNullOrEmpty(request.Search), e => EF.Functions.ILike(e.Name, $"%{request.Search}%"))
                                 .WhereIf(request.IsActive.HasValue, e => e.IsActive == request.IsActive!.Value);

            var categories = await query.OrderBy(e => e.Name)
                                        .Select(e => new CategoryForAdminDto
                                        {
                                            Id = e.Id,
                                            ParentId = e.ParentId,
                                            Name = e.Name,
                                            Slug = e.Slug,
                                            Description = e.Description,
                                            IsActive = e.IsActive,
                                            ProductCount = e.Products.Count
                                        })
                                        .ToListAsync(cancellationToken);
            return categories;
        }
    }
}
