using CommerceService.Application.Catalog.Categories.Queries.GetPublicCategories;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPublicCategories([FromQuery] GetPublicCategoriesQuery query, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
