namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    public class SetRolePermissionsCommand : IRequest
    {
        public Guid RoleId { get; set; }
        public required List<Guid> PermissionIds { get; set; }
    }
}
