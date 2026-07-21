using CommerceService.Application.Orders.Commands.ConfirmOrderPayment;
using CommerceService.Application.Orders.Commands.CreateOrder;
using CommerceService.Application.Orders.Commands.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
            [FromBody] CreateOrderRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand
            {
                IdempotencyKey = idempotencyKey,
                PaymentMethod = request.PaymentMethod,
                ShippingMethod = request.ShippingMethod,
                ShippingAddress = request.ShippingAddress,
                CustomerNote = request.CustomerNote,
                Items = request.Items,
                CouponCodes = request.CouponCodes
            };

            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{orderId:guid}/confirm-payment")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPayment(Guid orderId, CancellationToken cancellationToken)
        {
            await mediator.Send(new ConfirmOrderPaymentCommand { OrderId = orderId }, cancellationToken);
            return NoContent();
        }
    }
}
