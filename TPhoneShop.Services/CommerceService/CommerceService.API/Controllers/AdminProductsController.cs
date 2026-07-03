using CommerceService.Application.Catalog.Products.Commands.AddProductVariant;
using CommerceService.Application.Catalog.Products.Commands.CreateProduct;
using CommerceService.Application.Catalog.Products.Commands.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/products")]
    [ApiController]
    public class AdminProductsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.ProductsCreate)]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("{id}/variants")]
        [Authorize(Permissions.ProductsUpdate)]
        public async Task<IActionResult> AddProductVariant(Guid id, CreateProductVariantRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new AddProductVariantCommand
            {
                ProductId = id,
                Variant = request
            }, cancellationToken);
            return Ok();
        }
    }
}
