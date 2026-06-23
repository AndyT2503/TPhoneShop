using CommerceService.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    internal class SetRolePermissionsCommandHandler : IRequestHandler<SetRolePermissionsCommand>
    {
        private readonly CommerceDbContext _dbContext;
        private readonly IRolePermissionCache _rolePermissionCache;
        private readonly ILogger<SetRolePermissionsCommandHandler> _logger;
        public SetRolePermissionsCommandHandler(CommerceDbContext dbContext, IRolePermissionCache rolePermissionCache, ILogger<SetRolePermissionsCommandHandler> logger)
        {
            _dbContext = dbContext;
            _rolePermissionCache = rolePermissionCache;
            _logger = logger;
        }
        public async Task Handle(SetRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var currentPermissions = await _dbContext.RolePermissions
                                        .Where(x => x.RoleId == request.RoleId)
                                        .ToListAsync(cancellationToken);

            var currentPermissionIds = currentPermissions
                                            .Select(x => x.PermissionId)
                                            .ToHashSet();

            var requestedPermissionIds = request.PermissionIds
                .ToHashSet();

            var permissionsToAdd = requestedPermissionIds
                       .Except(currentPermissionIds)
                       .Select(permissionId => new RolePermission
                       {
                           RoleId = request.RoleId,
                           PermissionId = permissionId
                       });
            _dbContext.RolePermissions.AddRange(permissionsToAdd);

            var permissionsToRemove = currentPermissions
                        .Where(x => !requestedPermissionIds.Contains(x.PermissionId));
            _dbContext.RolePermissions.RemoveRange(permissionsToRemove);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await UpdateRolePermissionCache(request.RoleId, request.PermissionIds, cancellationToken);
        }

        private async Task UpdateRolePermissionCache(Guid roleId, List<Guid> requestedPermissionIds, CancellationToken cancellationToken)
        {
            try
            {
                var permissionNames = await _dbContext.Permissions
                    .Where(x => requestedPermissionIds.Contains(x.Id))
                    .Select(x => x.Name)
                    .ToHashSetAsync(cancellationToken);

                await _rolePermissionCache.SetAsync(
                    roleId,
                    permissionNames,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update permission cache for role {RoleId}",
                    roleId
                );
            }
        }
    }
}
