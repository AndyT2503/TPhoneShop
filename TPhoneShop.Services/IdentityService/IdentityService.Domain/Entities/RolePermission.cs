namespace IdentityService.Domain.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public required Role Role { get; set; }
        public Guid PermissionId { get; set; }
        public required Permission Permission { get; set; }
    }
}
