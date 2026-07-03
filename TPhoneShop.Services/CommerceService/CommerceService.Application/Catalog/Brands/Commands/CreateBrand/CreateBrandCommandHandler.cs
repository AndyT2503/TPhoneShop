using BuildingBlocks.Application.Slug;

namespace CommerceService.Application.Catalog.Brands.Commands.CreateBrand
{
    internal class CreateBrandCommandHandler(CommerceDbContext dbContext, ISlugGenerator slugGenerator) : IRequestHandler<CreateBrandCommand>
    {
        public async Task Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = new Brand
            {
                Name = request.Name,
                Description = request.Description,
                LogoId = request.LogoId,
                Slug = slugGenerator.Generate(request.Name),
                IsActive = true
            };

            dbContext.Brands.Add(brand);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
