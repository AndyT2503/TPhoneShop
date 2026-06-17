using IdentityService.Domain.Constants;
using System.Text.Json;

namespace IdentityService.Domain.Entities
{
    public class OutboxMessage : BaseEntity
    {
        public required string Type { get; set; }
        public required JsonDocument Payload { get; set; }
        public string Status { get; set; } = OutboxStatus.Pending;
        public int RetryCount { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public string? Error { get; set; }
    }
}
