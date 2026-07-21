using BuildingBlocks.Domain.Exceptions;
using CommerceService.Domain.ValueObjects;

namespace CommerceService.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;
        public required string ProductName { get; set; }
        public required string Sku { get; set; }
        public required Money UnitPrice { get; set; }
        public int Quantity { get; set; }
        public required Money SubTotal { get; set; }
        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Số lượng sản phẩm phải lớn hơn 0.");

            Quantity += quantity;

            RecalculateSubtotal();
        }

        public void DecreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Số lượng sản phẩm phải lớn hơn 0.");

            if (Quantity - quantity <= 0)
                throw new DomainException("Số lượng sản phẩm phải lớn hơn 0.");

            Quantity -= quantity;

            RecalculateSubtotal();
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Số lượng sản phẩm phải lớn hơn 0.");

            Quantity = quantity;

            RecalculateSubtotal();
        }

        public void UpdateUnitPrice(Money unitPrice)
        {
            UnitPrice = unitPrice;

            RecalculateSubtotal();
        }

        private void RecalculateSubtotal()
        {
            SubTotal = UnitPrice * Quantity;
        }
    }
}