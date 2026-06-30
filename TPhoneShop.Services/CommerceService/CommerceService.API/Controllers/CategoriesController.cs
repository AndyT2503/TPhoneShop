using CommerceService.Application.Catalog.Categories.Commands.CreateCategory;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.CategoriesCreate)]
        public async Task<IActionResult> CreateCategoy(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
