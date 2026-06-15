namespace IdentityService.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
