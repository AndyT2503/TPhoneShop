using BuildingBlocks.Application.Pagination;
using BuildingBlocks.Infrastructure.Extensions;
using CommerceService.Application.Catalog.Categories.Queries.Dtos;
using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Application.Catalog.Categories.Queries.GetPublicCategories
{
    internal class GetPublicCategoriesQueryHandler(CommerceDbContext dbContext, IMediaService mediaService) : IRequestHandler<GetPublicCategoriesQuery, IReadOnlyCollection<CategoryDto>>
    {
        public async Task<IReadOnlyCollection<CategoryDto>> Handle(GetPublicCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await dbContext.Categories
                                 .AsNoTracking()
                                 .Where(e => e.IsActive)
                                 .OrderBy(e => e.Name)
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
