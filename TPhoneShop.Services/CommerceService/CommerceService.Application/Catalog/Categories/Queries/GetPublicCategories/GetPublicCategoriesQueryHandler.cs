using BuildingBlocks.Infrastructure.Extensions;
using CommerceService.Application.Catalog.Categories.Queries.Dtos;
using CommerceService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommerceService.Application.Catalog.Categories.Queries.GetPublicCategories
{
    public class GetPublicCategoriesQueryHandler(CommerceDbContext dbContext) : IRequestHandler<GetPublicCategoriesQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetPublicCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Categories
                                 .AsNoTracking()
                                 .Where(e => e.IsActive)
                                 .WhereIf(!string.IsNullOrEmpty(request.Search), e => EF.Functions.ILike(e.Name, $"%{request.Search}%"));

            var categories = await query.OrderBy(e => e.Name)
                                        .Select(e => new CategoryDto
                                        {
                                            Id = e.Id,
                                            ParentId = e.ParentId,
                                            Name = e.Name,
                                            Slug = e.Slug,
                                            Description = e.Description
                                        })
                                        .ToListAsync(cancellationToken);
            return categories;
        }
    }
}
