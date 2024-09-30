namespace Documancer.Application.Features.KeyValues.Caching;

public static class KeyValueCacheKey
{
    #region Properties and Fields

    public const string GetAllCacheKey = "all-keyvalues";
    public const string PicklistCacheKey = "all-picklistcachekey";

    private static readonly TimeSpan RefreshInterval = TimeSpan.FromHours(1);
    private static readonly object _tokenLock = new();
    private static CancellationTokenSource _tokenSource = new(RefreshInterval);

    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(GetOrCreateTokenSource().Token));

    #endregion

    #region Methods

    public static string GetCacheKey(string name)
    {
        return $"{name}-keyvalues";
    }

    public static CancellationTokenSource GetOrCreateTokenSource()
    {
        lock (_tokenLock)
        {
            if (_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource(RefreshInterval);
            }
            
            return _tokenSource;
        }
    }

    public static void Refresh()
    {
        lock (_tokenLock)
        {
            if (!_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource(RefreshInterval);
            }
        }
    }

    #endregion
}