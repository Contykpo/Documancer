namespace Documancer.Application.Common.Interfaces.Caching
{
    public interface ICacheableRequest<TResponse> : IRequest<TResponse>
    {
        string CacheKey => string.Empty;
        MemoryCacheEntryOptions? Options { get; }
    }

    public interface IFusionCacheRequest<TResponse> : IRequest<TResponse>
    {
        string CacheKey => string.Empty;
        string CacheName => string.Empty;
    }
}