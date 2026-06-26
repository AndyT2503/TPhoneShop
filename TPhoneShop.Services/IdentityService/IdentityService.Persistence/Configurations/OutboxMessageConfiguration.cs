using BuildingBlocks.Infrastructure.Configurations;

namespace IdentityService.Persistence.Configurations
{
    internal class OutboxMessageConfiguration : OutboxMessageConfigurationBase<OutboxMessage>
    {
        public OutboxMessageConfiguration() : base("outbox_messages")
        {
        }
    }
}
