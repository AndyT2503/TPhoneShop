namespace CommerceService.Application.Catalog.ProductGroups.Commands.CreateProductGroup
{
    internal class CreateProductGroupCommandValidator : AbstractValidator<CreateProductGroupCommand>
    {
        public CreateProductGroupCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName("Tên nhóm sản phẩm");
        }
    }
}
