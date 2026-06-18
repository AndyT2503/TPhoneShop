using CommerceService.API.Models.Roles;
using CommerceService.Application.Authorization.Commands.CreateRole;
using CommerceService.Application.Authorization.Commands.SetRolePermissions;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Permissions.RolesCreate)]
        public async Task<IActionResult> CreateRole(CreateRoleCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{roleId}/permissions")]
        [Authorize(Permissions.RolesAssignPermissions)]
        public async Task<IActionResult> SetRole(Guid roleId, SetRolePermissionsRequest request)
        {
            await _mediator.Send(new SetRolePermissionsCommand { RoleId = roleId, PermissionIds = request.PermissionIds });
            return Ok();
        }
    }
}
