namespace CommerceService.API.Models.Roles
{
    public class SetRolePermissionsRequest
    {
        public List<Guid> PermissionIds { get; set; } = [];
    }
}
