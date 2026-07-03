using CommerceService.Application.Catalog.ProductGroups.Commands.CreateProductGroup;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/product-groups")]
    [ApiController]
    public class AdminProductGroupsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.ProductsCreate)]
        public async Task<IActionResult> CreateProductGroup(CreateProductGroupCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
