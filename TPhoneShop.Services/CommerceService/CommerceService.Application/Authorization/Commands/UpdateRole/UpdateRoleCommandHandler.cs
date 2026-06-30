namespace CommerceService.Application.Authorization.Commands.UpdateRole
{
    internal class UpdateRoleCommandHandler(CommerceDbContext dbContext) : IRequestHandler<UpdateRoleCommand>
    {
        public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException("Vai trò không tồn tại.");
            }

            var isRoleExist = await dbContext.Roles.AsNoTracking().AnyAsync(e => e.Name == request.Name && e.Id != role.Id, cancellationToken);
            if (isRoleExist)
            {
                throw new BadRequestException($"Vai trò {request.Name} đã tồn tại.");
            }
            role.Name = request.Name;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
