using CommerceService.Application.Authorization.Queries.GetListPermission;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/permissions")]
    [ApiController]
    public class AdminPermissionsController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        [Authorize(Permissions.PermissionsRead)]
        public async Task<IActionResult> GetListPermission(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetListPermissionQuery(), cancellationToken));
        }
    }
}
