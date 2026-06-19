using CommerceService.Application.Common.Abstractions;
using CommerceService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommerceService.Infrastructure.Authorization
{
    internal class UserAuthorizationService
    {
        private readonly CommerceDbContext _dbContext;
        private readonly IUserRoleCache _userRoleCache;
        private readonly IRolePermissionCache _rolePermissionCache;

        public UserAuthorizationService(
            CommerceDbContext dbContext,
            IUserRoleCache userRoleCache,
            IRolePermissionCache rolePermissionCache)
        {
            _dbContext = dbContext;
            _userRoleCache = userRoleCache;
            _rolePermissionCache = rolePermissionCache;
        }

        public async Task<bool> HasPermissionAsync(Guid? userId, string permission, CancellationToken cancellationToken = default)
        {
            if (userId is null)
            {
                return false;
            }

            var roleId = await GetRoleIdAsync(userId.Value, cancellationToken);
            if (!roleId.HasValue)
            {
                return false;
            }

            var permissions = await GetPermissionsAsync(roleId.Value, cancellationToken);
            return permissions.Contains(permission);
        }

        private async Task<Guid?> GetRoleIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var roleId = await _userRoleCache.GetAsync(userId, cancellationToken);

            if (roleId.HasValue)
            {
                return roleId;
            }

            roleId = await _dbContext.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .FirstOrDefaultAsync(cancellationToken);

            if (!roleId.HasValue)
            {
                return null;
            }

            try
            {
                await _userRoleCache.SetAsync(userId, roleId.Value, cancellationToken);
            }
            catch
            {
            }

            return roleId;
        }

        private async Task<HashSet<string>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var permissions = await _rolePermissionCache.GetAsync(roleId, cancellationToken);

            if (permissions is not null)
            {
                return permissions;
            }

            permissions = await _dbContext.RolePermissions
                .Where(x => x.RoleId == roleId)
                .Select(x => x.Permission.Name)
                .ToHashSetAsync(cancellationToken);

            try
            {
                await _rolePermissionCache.SetAsync(roleId, permissions, cancellationToken);
            }
            catch
            {
            }

            return permissions;
        }
    }
}
