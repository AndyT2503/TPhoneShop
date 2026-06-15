namespace IdentityService.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
