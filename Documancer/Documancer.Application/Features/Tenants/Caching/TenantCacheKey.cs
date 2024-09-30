namespace Documancer.Application.Features.Tenants.Caching
{
    public static class TenantCacheKey
    {
        #region Properties and Fields

        public const string GetAllCacheKey = "all-Tenants";
        public const string TenantsCacheKey = "all-TenantsCacheKey";

        private static readonly TimeSpan RefreshInterval = TimeSpan.FromHours(1);
        private static readonly object _tokenLock = new();

        private static CancellationTokenSource _tokenSource = new(RefreshInterval);

        public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(GetOrCreateTokenSource().Token));

        #endregion

        #region Methods

        public static string GetPaginationCacheKey(string parameters)
        {
            return $"TenantsWithPaginationQuery,{parameters}";
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
}