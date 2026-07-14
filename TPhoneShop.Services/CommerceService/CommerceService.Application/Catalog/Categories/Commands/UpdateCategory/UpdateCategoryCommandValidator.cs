namespace CommerceService.Application.Catalog.Categories.Commands.UpdateCategory
{
    internal class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithName("Tên danh mục");
        }
    }
}
