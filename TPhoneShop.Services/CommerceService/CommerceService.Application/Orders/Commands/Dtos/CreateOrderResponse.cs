namespace CommerceService.Application.Orders.Commands.Dtos
{
    public class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public required string OrderNumber { get; set; }
        public long TotalAmount { get; set; }
    }
}
