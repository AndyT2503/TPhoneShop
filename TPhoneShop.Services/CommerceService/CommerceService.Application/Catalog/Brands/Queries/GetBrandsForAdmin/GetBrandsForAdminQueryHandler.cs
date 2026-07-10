using BuildingBlocks.Application.Pagination;
using BuildingBlocks.Infrastructure.Extensions;
using CommerceService.Application.Catalog.Brands.Queries.Dtos;
using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Application.Catalog.Brands.Queries.GetBrandsForAdmin
{
    internal class GetBrandsForAdminQueryHandler(CommerceDbContext dbContext, IMediaService mediaService) : IRequestHandler<GetBrandsForAdminQuery, PagingResponse<BrandForAdminDto>>
    {
        public async Task<PagingResponse<BrandForAdminDto>> Handle(GetBrandsForAdminQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Brands
                                 .AsNoTracking()
                                 .WhereIf(!string.IsNullOrEmpty(request.Search), e => EF.Functions.ILike(e.Name, $"%{request.Search}%"))
                                 .Where(e => e.IsActive == request.IsActive)
                                 .OrderBy(e => e.Name)
                                 .Select(e => new
                                 {
                                     e.Id,
                                     e.Name,
                                     e.Slug,
                                     e.Description,
                                     e.LogoId,
                                     e.IsActive
                                 });
            var totalCount = await query.CountAsync(cancellationToken);
            var brands = await query.Paginate(request.PageNumber, request.PageSize)
                                    .ToListAsync(cancellationToken);

            var items = await Task.WhenAll(brands.Select(async brand => new BrandForAdminDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                Description = brand.Description,
                LogoUrl = await mediaService.GetPresignedUrl(brand.LogoId, cancellationToken),
                LogoId = brand.LogoId,
                IsActive = brand.IsActive
            }));

            return new PagingResponse<BrandForAdminDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
