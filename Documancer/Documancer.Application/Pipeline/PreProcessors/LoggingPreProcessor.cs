namespace Documancer.Application.Pipeline.PreProcessors
{
    public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        #region Properties and Fields

        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        public LoggingPreProcessor(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        #endregion

        #region Methods

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = nameof(TRequest);
            var userName = _currentUserService.UserName;

            _logger.LogTrace("Processing request of type {RequestName} with details {@Request} by user {UserName}", requestName, request, userName);

            return Task.CompletedTask;
        }

        #endregion
    }
}