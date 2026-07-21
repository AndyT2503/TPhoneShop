using CommerceService.Application.Orders.Commands.Dtos;

namespace CommerceService.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : CreateOrderRequest, IRequest<CreateOrderResponse>
    {
        public required string IdempotencyKey { get; set; }
    }
}
