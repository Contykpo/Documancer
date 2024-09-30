namespace Documancer.Application.Common.Interfaces.Caching
{
    public interface ICacheInvalidatorRequest<TResponse> : IRequest<TResponse>
    {
        string CacheKey => string.Empty;
        CancellationTokenSource? SharedExpiryTokenSource { get; }
    }

    public interface IFusionCacheRefreshRequest<TResponse> : IRequest<TResponse>
    {
        string CacheKey => string.Empty;
        string CacheName => string.Empty;
    }
}