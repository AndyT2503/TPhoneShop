using CommerceService.Application.Authorization.Queries.GetListPermission;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermissionsController(IMediator mediator)
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
