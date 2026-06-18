namespace CommerceService.Application.Common.Abstractions
{
    public interface IRolePermissionCache
    {
        Task<HashSet<string>?> GetAsync(Guid roleId, CancellationToken cancellationToken = default);

        Task SetAsync(Guid roleId, HashSet<string> permissions, CancellationToken cancellationToken = default);

        Task RemoveAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}
