using CommerceService.Application.Catalog.Brands.Commands.CreateBrand;

namespace CommerceService.Application.Catalog.Brands.Commands.UpdateBrand
{
    internal sealed class UpdateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public UpdateBrandCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithName("Tên");

            RuleFor(x => x.Description)
                .NotEmpty().WithName("Mô tả");

            RuleFor(x => x.LogoId)
                .NotEmpty().WithName("Logo");
        }
    }
}
