using BuildingBlocks.Application.Slug;

namespace CommerceService.Application.Catalog.Brands.Commands.UpdateBrand
{
    internal class UpdateBrandCommandHandler(CommerceDbContext dbContext, ISlugGenerator slugGenerator) : IRequestHandler<UpdateBrandCommand>
    {
        public async Task Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await dbContext.Brands.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (brand is null)
            {
                throw new NotFoundException("Nhãn hàng không tồn tại.");
            }
            if (brand.Name != request.Name)
            {
                brand.Slug = slugGenerator.Generate(request.Name);
            }
            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.LogoId = request.LogoId;
            brand.IsActive = request.IsActive;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
