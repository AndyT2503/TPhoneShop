namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    public class SetRolePermissionsCommandHandler : IRequestHandler<SetRolePermissionsCommand>
    {
        private readonly CommerceDbContext _dbContext;
        public SetRolePermissionsCommandHandler(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Handle(SetRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var currentPermissions = await _dbContext.RolePermissions
                                        .Where(x => x.RoleId == request.RoleId)
                                        .ToListAsync(cancellationToken);

            var currentPermissionIds = currentPermissions
                                            .Select(x => x.PermissionId)
                                            .ToHashSet();

            var requestedPermissionIds = request.PermissionIds
                .ToHashSet();

            var permissionsToAdd = requestedPermissionIds
                       .Except(currentPermissionIds)
                       .Select(permissionId => new RolePermission
                       {
                           RoleId = request.RoleId,
                           PermissionId = permissionId
                       });
            _dbContext.RolePermissions.AddRange(permissionsToAdd);

            var permissionsToRemove = currentPermissions
                        .Where(x => !requestedPermissionIds.Contains(x.PermissionId));
            _dbContext.RolePermissions.RemoveRange(permissionsToRemove);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
