using CommerceService.Persistence;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Application.Exceptions;

namespace CommerceService.Application.Catalog.Categories.Commands.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler(CommerceDbContext dbContext) : IRequestHandler<DeleteCategoryCommand>
    {
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException("Danh mục không tồn tại");
            }

            var hasSubCategories = await dbContext.Categories
                .AnyAsync(x => x.ParentId == request.Id, cancellationToken);
            if (hasSubCategories)
            {
                throw new BadRequestException("Không thể xóa danh mục này vì đang có danh mục con.");
            }

            var hasProducts = await dbContext.Products
                .AnyAsync(x => x.CategoryId == request.Id, cancellationToken);
            if (hasProducts)
            {
                throw new BadRequestException("Không thể xóa danh mục này vì đang chứa sản phẩm.");
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
