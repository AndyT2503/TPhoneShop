using CommerceService.Domain.Events.Role;
using System.Text.Json;

namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    internal class SetRolePermissionsCommandHandler(CommerceDbContext dbContext) : IRequestHandler<SetRolePermissionsCommand>
    {
        public async Task Handle(SetRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var currentPermissions = await dbContext.RolePermissions
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
            dbContext.RolePermissions.AddRange(permissionsToAdd);

            var permissionsToRemove = currentPermissions
                        .Where(x => !requestedPermissionIds.Contains(x.PermissionId));
            dbContext.RolePermissions.RemoveRange(permissionsToRemove);
            dbContext.OutboxMessages.Add(new OutboxMessage
            {
                Type = RolePermissionsUpdatedEvent.EventName,
                Payload = JsonSerializer.SerializeToDocument(new RolePermissionsUpdatedEvent(request.RoleId, request.PermissionIds))
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
