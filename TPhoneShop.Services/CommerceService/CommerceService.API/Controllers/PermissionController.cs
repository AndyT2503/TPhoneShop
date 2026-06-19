using CommerceService.Application.Authorization.Queries.GetListPermission;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/permissions")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Permissions.PermissionsRead)]
        public async Task<IActionResult> GetListPermission(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetListPermissionQuery(), cancellationToken));
        }
    }
}
