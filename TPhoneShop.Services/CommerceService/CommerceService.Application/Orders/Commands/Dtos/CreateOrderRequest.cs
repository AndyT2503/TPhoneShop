namespace CommerceService.Application.Orders.Commands.Dtos
{
    public class CreateOrderRequest
    {
        public required string PaymentMethod { get; set; }
        public required string ShippingMethod { get; set; }
        public required ShippingAddressRequest ShippingAddress { get; set; }
        public string? CustomerNote { get; set; }
        public List<OrderItemRequest> Items { get; set; } = [];
        public List<string> CouponCodes { get; set; } = [];
    }
}
