namespace CommerceService.Application.Common.Abstractions
{
    public interface IUserRoleCache
    {
        Task<Guid?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

        Task SetAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);

        Task RemoveAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
