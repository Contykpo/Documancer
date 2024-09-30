namespace Documancer.Application.Features.Loggers.Caching
{
    public static class LogsCacheKey
    {
        #region Properties and Fields

        public const string GetAllCacheKey = "all-logs";
        private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(30);
        private static readonly object _tokenLock = new();
        private static CancellationTokenSource _tokenSource = new(RefreshInterval);

        public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(GetOrCreateTokenSource().Token));


        #endregion

        #region Methods

        public static string GetChartDataCacheKey(string parameters)
        {
            return $"GetChartDataCacheKey,{parameters}";
        }

        public static string GetPaginationCacheKey(string parameters)
        {
            return $"LogsTrailsWithPaginationQuery,{parameters}";
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