using CommerceService.Application.Catalog.Categories.Commands.CreateCategory;
using CommerceService.Application.Catalog.Categories.Commands.UpdateCategory;
using CommerceService.Application.Catalog.Categories.Commands.DeleteCategory;
using CommerceService.Application.Catalog.Categories.Queries;
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
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQueryCommand query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Permissions.CategoriesUpdate)]
        public async Task<IActionResult> UpdateCategory(Guid id,UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            await mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Permissions.CategoriesDelete)]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            return NoContent();
        }
    }
}

