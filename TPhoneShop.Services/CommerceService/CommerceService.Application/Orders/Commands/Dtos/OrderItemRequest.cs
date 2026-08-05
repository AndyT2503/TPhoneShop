namespace CommerceService.Application.Orders.Commands.Dtos
{
    public class OrderItemRequest
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
