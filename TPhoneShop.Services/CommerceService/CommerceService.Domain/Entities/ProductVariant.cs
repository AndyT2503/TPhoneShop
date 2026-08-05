using CommerceService.Domain.ValueObjects;

namespace CommerceService.Domain.Entities
{
    public class ProductVariant : BaseEntity, ISoftDeletable
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public required string Sku { get; set; }
        public Guid ThumbnailId { get; set; }
        public required Money Price { get; set; }

        /// <summary>
        /// Original/list price before discount.
        /// Null if the variant is not on sale.
        /// </summary>
        public Money? CompareAtPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public Product Product { get; set; } = null!;
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

    }
}
