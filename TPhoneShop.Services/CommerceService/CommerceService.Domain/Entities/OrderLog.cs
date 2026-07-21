namespace CommerceService.Domain.Entities
{
    public class OrderLog : BaseEntity, ISoftDeletable
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.OrderLogAction"/>.
        /// </summary>
        public required string Action { get; set; }

        /// <summary>
        /// Snapshot status of Order after complete.
        /// </summary>
        public required string Status { get; set; }
        public required string PaymentMethod { get; set; }
        public required string PaymentStatus { get; set; }
        public required string ShippingStatus { get; set; }
        public required string ShippingMethod { get; set; }
        public Guid PerformedBy { get; set; }
        public DateTimeOffset PerfomedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
