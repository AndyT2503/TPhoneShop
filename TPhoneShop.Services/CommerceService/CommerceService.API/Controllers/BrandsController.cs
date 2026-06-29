using CommerceService.Application.Catalog.Brands.Commands.CreateBrand;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
