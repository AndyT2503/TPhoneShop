using CommerceService.Domain.Constants;

namespace CommerceService.Application.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
    {
        private static readonly string[] AllowedDiscountTypes =
            [DiscountTypes.FixedAmount, DiscountTypes.Percentage];

        public CreateCouponCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã giảm giá không được để trống.")
                .MaximumLength(50).WithMessage("Mã giảm giá không được quá 50 ký tự.");

            RuleFor(x => x.DiscountType)
                .NotEmpty().WithMessage("Loại giảm giá không được để trống.")
                .Must(x => AllowedDiscountTypes.Contains(x))
                .WithMessage("Loại giảm giá không hợp lệ.");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Giá trị giảm giá phải lớn hơn 0.");

            When(x => x.DiscountType == DiscountTypes.Percentage, () =>
            {
                RuleFor(x => x.DiscountValue)
                    .LessThanOrEqualTo(100).WithMessage("Phần trăm giảm giá không được vượt quá 100.");
            });

            RuleFor(x => x.StartsAt)
                .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống.");

            When(x => x.ExpiresAt.HasValue, () =>
            {
                RuleFor(x => x.ExpiresAt)
                    .GreaterThan(x => x.StartsAt)
                    .WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");
            });

            When(x => x.UsageLimit.HasValue, () =>
            {
                RuleFor(x => x.UsageLimit)
                    .GreaterThan(0).WithMessage("Giới hạn sử dụng phải lớn hơn 0.");
            });

            When(x => x.PerUserUsageLimit.HasValue, () =>
            {
                RuleFor(x => x.PerUserUsageLimit)
                    .GreaterThan(0).WithMessage("Giới hạn sử dụng mỗi người phải lớn hơn 0.");
            });
        }
    }
}
