using CommerceService.Application.Authorization.Commands.CreateRole;
using CommerceService.Application.Authorization.Commands.DeleteRole;
using CommerceService.Application.Authorization.Commands.Dtos;
using CommerceService.Application.Authorization.Commands.SetRolePermissions;
using CommerceService.Application.Authorization.Commands.UpdateRole;
using CommerceService.Application.Authorization.Queries.GetListRole;
using CommerceService.Application.Authorization.Queries.GetPermissionsByRole;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/roles")]
    [ApiController]
    public class AdminRolesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.RolesCreate)]
        public async Task<IActionResult> CreateRole(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Authorize(Permissions.RolesRead)]
        public async Task<IActionResult> GetListRole(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetListRoleQuery(), cancellationToken));
        }

        [HttpGet("{roleId}/permissions")]
        [Authorize(Permissions.RolesRead)]
        public async Task<IActionResult> GetListPermissionByRole(Guid roleId, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetPermissionsByRoleQuery(roleId), cancellationToken));
        }

        [HttpPost("{roleId}/permissions")]
        [Authorize(Permissions.RolesAssignPermissions)]
        public async Task<IActionResult> SetRole(Guid roleId, SetRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new SetRolePermissionsCommand { RoleId = roleId, PermissionIds = request.PermissionIds }, cancellationToken);
            return Ok();
        }

        [HttpPut("{roleId}")]
        [Authorize(Permissions.RolesUpdate)]
        public async Task<IActionResult> UpdateRole(Guid roleId, UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateRoleCommand { Id = roleId, Name = request.Name }, cancellationToken);
            return Ok();
        }

        [HttpDelete("{roleId}")]
        [Authorize(Permissions.RolesDelete)]
        public async Task<IActionResult> DeleteRole(Guid roleId, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteRoleCommand(roleId), cancellationToken);
            return Ok();
        }
    }
}
