using CommerceService.Application.Catalog.Brands.Commands.CreateBrand;
using CommerceService.Application.Catalog.Brands.Commands.DeleteBrand;
using CommerceService.Application.Catalog.Brands.Commands.Dtos;
using CommerceService.Application.Catalog.Brands.Commands.UpdateBrand;
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

        [HttpPut("{id}")]
        [Authorize(Permissions.BrandsUpdate)]
        public async Task<IActionResult> UpdateBrand(Guid id, UpdateBrandRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateBrandCommand
            {
                Id = id,
                Description = request.Description,
                Name = request.Name,
                IsActive = request.IsActive,
                LogoId = request.LogoId
            }, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Permissions.BrandsDelete)]
        public async Task<IActionResult> DeletBrand(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteBrandCommand(id), cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Authorize(Permissions.BrandsRead)]
        public async Task<IActionResult> GetBrands([FromQuery] GetBrandsForAdminQuery query, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
