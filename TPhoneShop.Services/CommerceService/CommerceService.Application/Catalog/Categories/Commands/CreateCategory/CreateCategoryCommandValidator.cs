namespace CommerceService.Application.Catalog.Categories.Commands.CreateCategory
{
    internal class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithName("Tên danh mục");
        }
    }
}
