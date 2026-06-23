namespace CommerceService.Application.Authorization.Commands.DeleteRole
{
    internal class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly CommerceDbContext _dbContext;
        public DeleteRoleCommandHandler(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Id == request.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException("Vai trò không tồn tại.");
            }
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
