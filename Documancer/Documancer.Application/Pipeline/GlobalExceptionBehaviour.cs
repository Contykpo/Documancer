namespace Documancer.Application.Pipeline
{
    public class GlobalExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        #region Properties and Fields

        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<TRequest> _logger;

        #endregion

        #region Constructors

        public GlobalExceptionBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        #endregion

        #region Methods

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                var requestName = typeof(TRequest).Name;
                var userName = _currentUserService.UserName;

                _logger.LogError(exception,
                    "Request: {RequestName} by User: {UserName} failed. Error: {errorMesage}. Request Details: {Request}",
                    requestName,
                    userName,
                    exception.Message,
                    request);

                throw;
            }
        }

        #endregion
    }
}