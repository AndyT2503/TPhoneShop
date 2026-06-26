using CommerceService.Application.Catalog.ProductGroups.Commands.CreateProductGroup;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupsController(IMediator mediator) : ControllerBase
    {
        public async Task<IActionResult> CreateProductGroup(CreateProductGroupCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
