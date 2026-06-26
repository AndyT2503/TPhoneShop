using BuildingBlocks.Infrastructure.Configurations;

namespace CommerceService.Persistence.Configurations
{
    internal class OutboxMessageConfiguration : OutboxMessageConfigurationBase<OutboxMessage>
    {
        public OutboxMessageConfiguration() : base("outbox_messages", CommerceDbContext.EventsSchema) { }
    }
}
