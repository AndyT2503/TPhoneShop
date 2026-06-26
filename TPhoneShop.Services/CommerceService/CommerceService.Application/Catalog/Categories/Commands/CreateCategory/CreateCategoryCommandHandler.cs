using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Application.Catalog.Categories.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandHandler(CommerceDbContext dbContext, ISlugGenerator slugGenerator) : IRequestHandler<CreateCategoryCommand>
    {
        public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                ParentId = request.ParentId,
                Name = request.Name,
                Description = request.Description,
                Slug = slugGenerator.Generate(request.Name),
                IsActive = true
            };

            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
