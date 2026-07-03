using CommerceService.Application.Catalog.Brands.Commands.CreateBrand;
using CommerceService.Application.Catalog.Brands.Queries.GetPublicBrands;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.BrandsCreate)]
        public async Task<IActionResult> CreateBrand(CreateBrandCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicBrands([FromQuery] GetPublicBrandsQuery query, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
