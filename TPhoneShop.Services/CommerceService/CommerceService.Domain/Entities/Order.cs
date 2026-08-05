using BuildingBlocks.Domain.Exceptions;
using CommerceService.Domain.Constants;
using CommerceService.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace CommerceService.Domain.Entities
{
    public class Order : BaseEntity, ISoftDeletable
    {
        public Guid CustomerId { get; set; }
        public required string OrderNumber { get; set; }
        public required string Status { get; set; }
        /// <summary>
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.PaymentMethods"/>.
        /// </summary>
        public required string PaymentMethod { get; set; }
        /// <summary>
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.PaymentStatuses"/>.
        /// </summary>
        public required string PaymentStatus { get; set; }
        /// <summary>
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.ShippingStatuses"/>.
        /// </summary>
        public required string ShippingStatus { get; set; }
        /// <summary>
        /// Must be one of the constants defined in <see cref="CommerceService.Domain.Constants.ShippingMethods"/>.
        /// </summary>
        public required string ShippingMethod { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
        public string? CustomerNote { get; set; }
        public string? CancelReason { get; set; }
        public Money? TotalDiscount { get; set; }
        public Money? ShippingFee { get; set; }
        public required Money Tax { get; set; }
        /// <summary>
        /// Total value of all order items before applying discounts, taxes, or shipping fees.
        /// </summary>
        public required Money SubTotal { get; set; }

        /// <summary>
        /// Final amount the customer must pay after applying discounts, taxes, and shipping fees.
        /// </summary>
        public required Money TotalAmount { get; set; }
        public DateTimeOffset? ShippedAt { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? PaidAt { get; set; }
        public ICollection<OrderDiscount> OrderDiscounts { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public ICollection<OrderLog> OrderLogs { get; set; } = [];

        private Order()
        {
        }

        [SetsRequiredMembers]
        public Order(
         Guid customerId,
         string orderNumber,
         string paymentMethod,
         string shippingMethod,
         ShippingAddress shippingAddress,
         IEnumerable<OrderItem> items,
         string? customerNote = null
     )
        {
            if (!items.Any())
                throw new DomainException("Đơn hàng phải có ít nhất một sản phẩm.");

            CustomerId = customerId;
            OrderNumber = orderNumber;
            Status = OrderStatuses.Pending;
            PaymentMethod = paymentMethod;
            PaymentStatus = PaymentStatuses.Pending;
            ShippingMethod = shippingMethod;
            ShippingStatus = ShippingStatuses.Pending;
            ShippingAddress = shippingAddress;
            CustomerNote = customerNote;
            Tax = Money.Zero;
            ShippingFee = Money.Zero;
            TotalDiscount = Money.Zero;
            TotalAmount = Money.Zero;
            SubTotal = Money.Zero;
            foreach (var item in items)
            {
                OrderItems.Add(item);
            }
            RecalculateTotals();
        }

        #region Item

        public void AddItem(OrderItem item)
        {
            EnsureCanModify();

            var existingItem = OrderItems.FirstOrDefault(x =>
                x.ProductVariantId == item.ProductVariantId);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(item.Quantity);
            }
            else
            {
                OrderItems.Add(item);
            }

            RecalculateTotals();
        }

        public void RemoveItem(Guid productVariantId)
        {
            EnsureCanModify();

            var item = OrderItems.FirstOrDefault(x =>
                x.ProductVariantId == productVariantId);

            if (item == null)
                throw new DomainException("Không tìm thấy sản phẩm trong đơn hàng.");

            OrderItems.Remove(item);
            RecalculateTotals();
        }

        public void UpdateItemQuantity(Guid productVariantId, int quantity)
        {
            EnsureCanModify();

            var item = OrderItems.FirstOrDefault(x =>
                x.ProductVariantId == productVariantId);

            if (item == null)
                throw new DomainException("Không tìm thấy sản phẩm trong đơn hàng.");

            item.UpdateQuantity(quantity);
            RecalculateTotals();
        }

        #endregion

        #region Discount

        public void ApplyDiscount(OrderDiscount discount)
        {
            EnsureCanModify();

            if (OrderDiscounts.Any(x => x.CouponId == discount.CouponId))
                throw new DomainException("Mã giảm giá đã được áp dụng.");

            OrderDiscounts.Add(discount);
            RecalculateDiscount();
            RecalculateTotals();
        }

        public void RemoveDiscount(Guid couponId)
        {
            EnsureCanModify();

            var discount = OrderDiscounts.FirstOrDefault(x =>
                x.CouponId == couponId);

            if (discount == null)
                throw new DomainException("Không tìm thấy mã giảm giá.");

            OrderDiscounts.Remove(discount);
            RecalculateDiscount();
            RecalculateTotals();
        }

        #endregion

        #region Shipping

        public void UpdateShippingAddress(ShippingAddress address)
        {
            EnsureCanModify();
            ShippingAddress = address;
        }

        public void UpdateShippingMethod(string shippingMethod)
        {
            EnsureCanModify();
            ShippingMethod = shippingMethod;
        }

        public void UpdateShippingFee(Money shippingFee)
        {
            EnsureCanModify();
            ShippingFee = shippingFee;
            RecalculateTotals();
        }

        public void Ship()
        {
            if (PaymentStatus != PaymentStatuses.Paid)
                throw new DomainException("Không thể giao đơn hàng khi chưa thanh toán.");

            if (ShippingStatus == ShippingStatuses.Shipped)
                throw new DomainException("Đơn hàng đã được giao.");

            ShippingStatus = ShippingStatuses.Shipped;
            ShippedAt = DateTimeOffset.UtcNow;
        }

        #endregion

        #region Payment

        public void MarkAsPaid()
        {
            if (PaymentStatus == PaymentStatuses.Paid)
                throw new DomainException("Đơn hàng đã được thanh toán.");

            PaymentStatus = PaymentStatuses.Paid;
            PaidAt = DateTimeOffset.UtcNow;
        }

        public void MarkAsRefunded()
        {
            PaymentStatus = PaymentStatuses.Refunded;
        }

        #endregion

        #region Status

        public void Confirm()
        {
            if (Status != OrderStatuses.Pending)
                throw new DomainException("Chỉ có thể xác nhận đơn hàng ở trạng thái chờ xử lý.");

            Status = OrderStatuses.Confirmed;
        }

        public void Complete()
        {
            if (ShippingStatus != ShippingStatuses.Shipped)
                throw new DomainException("Đơn hàng chưa được giao.");

            if (Status == OrderStatuses.Completed)
                throw new DomainException("Đơn hàng đã hoàn thành.");

            Status = OrderStatuses.Completed;
            CompletedAt = DateTimeOffset.UtcNow;
        }

        public void Cancel(string reason)
        {
            if (Status == OrderStatuses.Completed)
                throw new DomainException("Không thể hủy đơn hàng đã hoàn thành.");

            if (Status == OrderStatuses.Cancelled)
                throw new DomainException("Đơn hàng đã bị hủy.");

            Status = OrderStatuses.Cancelled;
            CancelReason = reason;
            CancelledAt = DateTimeOffset.UtcNow;
        }

        #endregion

        #region Log

        public void AddLog(string action, Guid performedBy)
        {
            OrderLogs.Add(new OrderLog
            {
                OrderId = Id,
                Action = action,
                Status = Status,
                PaymentMethod = PaymentMethod,
                PaymentStatus = PaymentStatus,
                ShippingStatus = ShippingStatus,
                ShippingMethod = ShippingMethod,
                PerformedBy = performedBy,
                PerfomedAt = DateTimeOffset.UtcNow
            });
        }

        #endregion

        #region Private

        private void EnsureCanModify()
        {
            if (Status != OrderStatuses.Pending)
                throw new DomainException("Không thể chỉnh sửa đơn hàng ở trạng thái hiện tại.");
        }

        private void RecalculateDiscount()
        {
            TotalDiscount = OrderDiscounts.Aggregate(
                Money.Zero,
                (current, discount) => current + discount.AppliedAmount);
        }

        private void RecalculateTotals()
        {
            SubTotal = OrderItems.Aggregate(
                Money.Zero,
                (current, item) => current + item.SubTotal);

            TotalAmount =
                SubTotal
                + (ShippingFee ?? Money.Zero)
                + Tax
                - (TotalDiscount ?? Money.Zero);
        }

        #endregion
    }
}