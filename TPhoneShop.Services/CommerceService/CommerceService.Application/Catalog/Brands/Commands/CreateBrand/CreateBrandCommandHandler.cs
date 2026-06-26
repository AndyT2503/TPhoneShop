using CommerceService.Application.Common.Abstractions;

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
                LogoUrl = request.LogoUrl,
                Slug = slugGenerator.Generate(request.Name),
                IsActive = true
            };

            dbContext.Brands.Add(brand);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
