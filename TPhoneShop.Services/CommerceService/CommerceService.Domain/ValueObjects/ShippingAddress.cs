namespace CommerceService.Domain.ValueObjects
{
    public class ShippingAddress
    {
        public required string RecipientName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string Province { get; set; }
        public required string Ward { get; set; }
        public required string Address { get; set; }
    }
}