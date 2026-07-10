namespace CommerceService.Domain.Entities
{
    public class ProductVariant : BaseEntity, ISoftDeletable
    {
        public Guid ProductId { get; set; }

        public required string Name { get; set; }

        public required string Sku { get; set; }

        public Guid ThumbnailId { get; set; }

        /// <summary>
        /// Stored as amount ×100
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Original/list price before discount.
        /// Null if the variant is not on sale.
        /// Stored as amount ×100
        /// </summary>
        public long? CompareAtPrice { get; set; }
        public string Currency { get; set; } = Constants.Currency.VND;

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public Product Product { get; set; } = null!;
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

    }
}
