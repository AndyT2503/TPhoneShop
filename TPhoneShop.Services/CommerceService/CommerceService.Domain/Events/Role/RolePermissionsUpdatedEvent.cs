using BuildingBlocks.Domain.Events;

namespace CommerceService.Domain.Events.Role
{
    public record RolePermissionsUpdatedEvent(Guid RoleId, List<Guid> PermissionIds) : IDomainEvent
    {
        public const string EventName = "role_permission_updated";
    }
}
