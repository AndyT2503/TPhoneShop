namespace CommerceService.Domain.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        /// <summary>
        /// Inventory action type.
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.InventoryActions"/>.
        /// </summary>
        public required string Action { get; set; }
        public int BeforeQuantity { get; set; }
        public int QuantityChanged { get; set; }
        public int AfterQuantity { get; set; }
        public Guid? OrderId { get; set; }
        public Guid PerformedBy { get; set; }
    }
}
