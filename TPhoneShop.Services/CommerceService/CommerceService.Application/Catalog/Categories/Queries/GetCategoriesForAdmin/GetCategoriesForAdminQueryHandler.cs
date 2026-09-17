using BuildingBlocks.Application.Pagination;
using BuildingBlocks.Infrastructure.Extensions;
using CommerceService.Application.Catalog.Categories.Queries.Dtos;
using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Application.Catalog.Categories.Queries.GetCategoriesForAdmin
{
    internal class GetCategoriesForAdminQueryHandler(CommerceDbContext dbContext, IMediaService mediaService) : IRequestHandler<GetCategoriesForAdminQuery, IReadOnlyCollection<CategoryForAdminDto>>
    {
        public async Task<IReadOnlyCollection<CategoryForAdminDto>> Handle(GetCategoriesForAdminQuery request, CancellationToken cancellationToken)
        {
            var categories = await dbContext.Categories
                                 .AsNoTracking()
                                 .WhereIf(!string.IsNullOrEmpty(request.Search), e => EF.Functions.ILike(e.Name, $"%{request.Search}%"))
                                 .Where(e => e.IsActive == request.IsActive)
                                 .OrderBy(e => e.Name)
                                 .Select(e => new CategoryForAdminDto
                                 {
                                     Id = e.Id,
                                     ParentId = e.ParentId,
                                     Name = e.Name,
                                     Slug = e.Slug,
                                     Description = e.Description,
                                     IsActive = e.IsActive
                                 })
                                 .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
