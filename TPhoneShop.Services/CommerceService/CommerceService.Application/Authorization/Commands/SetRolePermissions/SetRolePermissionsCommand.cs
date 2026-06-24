using CommerceService.Application.Authorization.Commands.Dtos;

namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    public class SetRolePermissionsCommand : SetRolePermissionsRequest, IRequest
    {
        public Guid RoleId { get; set; }
    }
}
