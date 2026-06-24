namespace CommerceService.Application.Authorization.Commands.Dtos
{
    public class SetRolePermissionsRequest
    {
        public List<Guid> PermissionIds { get; set; } = [];
    }
}
