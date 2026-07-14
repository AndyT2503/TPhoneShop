namespace CommerceService.Application.Catalog.Categories.Queries;

public class GetCategoriesQueryCommandHandler(CommerceDbContext dbContext) :IRequestHandler<GetCategoriesQueryCommand,List<CategoryData>>
{
    public async Task<List<CategoryData>> Handle(GetCategoriesQueryCommand request, CancellationToken cancellationToken)
    {
        var query = dbContext.Categories.AsNoTracking();
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{request.Search}%"));
        }
        var categories= await query.OrderBy(c=>c.Name).Select(c=>new CategoryData
        {
            Id=c.Id,
            ParentId=c.ParentId,
            Name=c.Name,
            Slug=c.Slug,
            Description=c.Description,
            IsActive=c.IsActive,
            ProductCount=c.Products.Count
        }).ToListAsync(cancellationToken);

        return categories;

    }
}