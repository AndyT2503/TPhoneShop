using CommerceService.Application.Catalog.Categories.Commands.CreateCategory;
using CommerceService.Application.Catalog.Categories.Commands.UpdateCategory;
using CommerceService.Application.Catalog.Categories.Commands.DeleteCategory;
using CommerceService.Application.Catalog.Categories.Commands.Dtos;
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
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesForAdminQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Permissions.CategoriesUpdate)]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequest request,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateCategoryCommand
            {
                Id = id,
                ParentId = request.ParentId,
                Name = request.Name,
                Description = request.Description
            }, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.CategoriesDelete)]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            return Ok();
        }
    }
}

