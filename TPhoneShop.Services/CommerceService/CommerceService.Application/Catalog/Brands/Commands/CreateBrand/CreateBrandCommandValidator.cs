namespace CommerceService.Application.Catalog.Brands.Commands.CreateBrand
{
    internal sealed class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
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
