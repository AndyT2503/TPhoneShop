namespace CommerceService.ReadModel.Catalog.Services.Product
{
    public interface IProductProjectionService
    {
        Task UpsertProductAsync(Guid productId, CancellationToken cancellationToken = default);

        Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}
