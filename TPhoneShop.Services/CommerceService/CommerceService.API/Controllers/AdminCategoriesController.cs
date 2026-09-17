using CommerceService.Application.Catalog.Categories.Commands.CreateCategory;
using CommerceService.Application.Catalog.Categories.Queries.GetCategoriesForAdmin;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/categories")]
    [ApiController]
    public class AdminCategoriesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.CategoriesCreate)]
        public async Task<IActionResult> CreateCategoy(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Authorize(Permissions.CategoriesRead)]
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesForAdminQuery query, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
