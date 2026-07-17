using BuildingBlocks.Application.Slug;
using BuildingBlocks.Application.Exceptions;
namespace CommerceService.Application.Catalog.Categories.Commands.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler(CommerceDbContext dbContext, ISlugGenerator slugGenerator) : IRequestHandler<UpdateCategoryCommand>
    {
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (category is null)
            {
                throw new NotFoundException("Danh mục không tồn tại.");
            }
            if (request.ParentId == request.Id)
            {
                throw new BadRequestException("Danh mục cha không thể chính là danh mục hiện tại.");
            }
            if (request.Name != category.Name)
            {
                var slug = slugGenerator.Generate(request.Name);
                var isSlugExists = await dbContext.Categories
                    .AnyAsync(x => x.Slug == slug && x.Id != request.Id, cancellationToken);
                if (isSlugExists)
                {
                    throw new BadRequestException("Tên danh mục đã tồn tại (trùng đường dẫn tĩnh/slug).");
                }
                category.Slug = slug;
            }
            if (request.ParentId.HasValue)
            {
                var parentExists = await dbContext.Categories.AnyAsync(x => x.Id == request.ParentId.Value, cancellationToken);
                if (!parentExists)
                {
                    throw new BadRequestException("Danh mục cha được chọn không tồn tại.");
                }
            }
            category.ParentId = request.ParentId;
            category.Name = request.Name;
            category.Description = request.Description;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}