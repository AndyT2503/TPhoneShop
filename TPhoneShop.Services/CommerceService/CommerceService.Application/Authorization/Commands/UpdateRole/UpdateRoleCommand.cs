using CommerceService.Application.Authorization.Commands.Dtos;

namespace CommerceService.Application.Authorization.Commands.UpdateRole
{
    public class UpdateRoleCommand : UpdateRoleRequest, IRequest
    {
        public Guid Id { get; set; }
    }
}
