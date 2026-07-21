namespace CommerceService.Application.Common.Abstractions
{
    public interface IIdempotencyCache
    {
        Task<T?> GetExistingResultAsync<T>(string key) where T : class;
        Task SaveResultAsync<T>(string key, T result, TimeSpan expiration) where T : class;
    }
}
