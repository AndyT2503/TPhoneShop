using CommerceService.Domain.Constants;

namespace CommerceService.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.IdempotencyKey)
                .NotEmpty().WithMessage("Idempotency key không được để trống.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Phương thức thanh toán không được để trống.")
                .Must(x => PaymentMethods.All.Contains(x))
                .WithMessage("Phương thức thanh toán không hợp lệ.");

            RuleFor(x => x.ShippingMethod)
                .NotEmpty().WithMessage("Phương thức vận chuyển không được để trống.")
                .Must(x => ShippingMethods.All.Contains(x))
                .WithMessage("Phương thức vận chuyển không hợp lệ.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Đơn hàng phải có ít nhất một sản phẩm.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductVariantId)
                    .NotEmpty().WithMessage("Mã sản phẩm không hợp lệ.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Số lượng sản phẩm phải lớn hơn 0.");
            });

            RuleFor(x => x.ShippingAddress)
                .NotNull().WithMessage("Địa chỉ giao hàng không được để trống.");

            When(x => x.ShippingAddress is not null, () =>
            {
                RuleFor(x => x.ShippingAddress.RecipientName)
                    .NotEmpty().WithMessage("Tên người nhận không được để trống.");

                RuleFor(x => x.ShippingAddress.PhoneNumber)
                    .NotEmpty().WithMessage("Số điện thoại không được để trống.");

                RuleFor(x => x.ShippingAddress.Province)
                    .NotEmpty().WithMessage("Tỉnh/Thành phố không được để trống.");

                RuleFor(x => x.ShippingAddress.Ward)
                    .NotEmpty().WithMessage("Phường/Xã không được để trống.");

                RuleFor(x => x.ShippingAddress.Address)
                    .NotEmpty().WithMessage("Địa chỉ không được để trống.");
            });
        }
    }
}
