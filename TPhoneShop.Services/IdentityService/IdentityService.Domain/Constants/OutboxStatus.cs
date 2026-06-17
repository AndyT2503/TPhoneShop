namespace IdentityService.Domain.Constants
{
    public static class OutboxStatus
    {
        public const string Pending = "pending";
        public const string Processed = "processed";
        public const string Failed = "failed";
        public const string Expired = "expired";
    }
}
