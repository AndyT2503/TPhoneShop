using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Application.Catalog.ProductGroups.Commands.CreateProductGroup
{
    internal sealed class CreateProductGroupCommandHandler(CommerceDbContext dbContext, ISlugGenerator slugGenerator) : IRequestHandler<CreateProductGroupCommand>
    {
        public async Task Handle(CreateProductGroupCommand request, CancellationToken cancellationToken)
        {
            var slug = slugGenerator.Generate(request.Name);

            var exists = await dbContext.ProductGroups.AnyAsync(x => x.Slug == slug, cancellationToken);

            if (exists)
            {
                throw new BadRequestException("Nhóm sản phẩm đã tồn tại.");
            }

            var productGroup = new ProductGroup
            {
                Name = request.Name,
                Slug = slug,
                IsActive = true
            };

            dbContext.ProductGroups.Add(productGroup);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
