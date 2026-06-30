using BuildingBlocks.Application.Events;
using CommerceService.Application.Common.Abstractions;
using CommerceService.Domain.Events.Role;
using Microsoft.Extensions.Logging;

namespace CommerceService.Application.Authorization.Events.RolePermissionsUpdatedEventHandlers
{
    internal class RolePermissionsCacheHandler(
        CommerceDbContext dbContext,
        ILogger<RolePermissionsCacheHandler> logger,
        IRolePermissionCache rolePermissionCache)
        : INotificationHandler<DomainEventNotification<RolePermissionsUpdatedEvent>>
    {
        public async Task Handle(DomainEventNotification<RolePermissionsUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            try
            {
                var permissionNames = await dbContext.Permissions
                    .Where(x => notification.Event.PermissionIds.Contains(x.Id))
                    .Select(x => x.Name)
                    .ToHashSetAsync(cancellationToken);

                await rolePermissionCache.SetAsync(
                    notification.Event.RoleId,
                    permissionNames,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to update permission cache for role {RoleId}",
                    notification.Event.RoleId
                );
            }
        }
    }
}
