namespace CommerceService.Application.Catalog.Categories.Queries;

public class GetCategoriesQueryCommand:IRequest<List<CategoryData>>
{
   public string? Search { get; set; }
}

public class CategoryData
{
    public Guid? Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null !;
    public string? Description  { get; set; }
    public bool IsActive { get; set; }
    public int ProductCount { get; set; }
}