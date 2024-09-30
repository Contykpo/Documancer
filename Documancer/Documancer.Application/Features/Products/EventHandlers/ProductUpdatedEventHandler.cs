namespace Documancer.Application.Features.Products.EventHandlers
{
    public class ProductUpdatedEventHandler : INotificationHandler<UpdatedEvent<Product>>
    {
        #region Properties and Fields

        private readonly ILogger<ProductUpdatedEventHandler> _logger;

        #endregion

        #region Constructors

        public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task Handle(UpdatedEvent<Product> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);

            return Task.CompletedTask;
        }

        #endregion
    }
}