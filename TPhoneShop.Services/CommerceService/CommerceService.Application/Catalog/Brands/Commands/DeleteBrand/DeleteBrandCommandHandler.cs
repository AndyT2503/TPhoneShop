namespace CommerceService.Application.Catalog.Brands.Commands.DeleteBrand
{
    internal class DeleteBrandCommandHandler(CommerceDbContext dbContext) : IRequestHandler<DeleteBrandCommand>
    {
        public async Task Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await dbContext.Brands.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (brand is null)
            {
                throw new NotFoundException("Nhãn hàng không tồn tại.");
            }
            dbContext.Brands.Remove(brand);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
