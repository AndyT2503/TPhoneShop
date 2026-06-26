namespace CommerceService.Application.Catalog.Products.Commands.CreateProduct
{
    internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.BrandId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
