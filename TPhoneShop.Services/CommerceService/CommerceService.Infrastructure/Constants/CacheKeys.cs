namespace CommerceService.Infrastructure.Constants
{
    internal static class CacheKeys
    {
        public static string UserRoles(Guid userId) => $"user:{userId}:roles";
        public static string RolePermissions(Guid roleId) => $"role:{roleId}:permissions";
    }
}
