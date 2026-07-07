using CommerceService.Application.Catalog.Brands.Commands.CreateBrand;
using CommerceService.Application.Catalog.Brands.Queries.GetBrandsForAdmin;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/brands")]
    [ApiController]
    public class AdminBrandsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.BrandsCreate)]
        public async Task<IActionResult> CreateBrand(CreateBrandCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Authorize(Permissions.BrandsRead)]
        public async Task<IActionResult> GetBrands(GetBrandsForAdminQuery query, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
