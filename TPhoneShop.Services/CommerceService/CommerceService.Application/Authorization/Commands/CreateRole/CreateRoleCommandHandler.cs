namespace CommerceService.Application.Authorization.Commands.CreateRole
{
    internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand>
    {
        private readonly CommerceDbContext _dbContext;
        public CreateRoleCommandHandler(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var isRoleExist = await _dbContext.Roles.AnyAsync(e => e.Name == request.Name, cancellationToken);
            if (isRoleExist)
            {
                throw new BadRequestException($"Vai trò {request.Name} đã tồn tại");
            }
            _dbContext.Roles.Add(new Role { Name = request.Name });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
