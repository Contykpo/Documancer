namespace Documancer.Application.Features.Products.EventHandlers
{
    public class ProductDeletedEventHandler : INotificationHandler<DeletedEvent<Product>>
    {
        #region Properties and Fields

        private readonly ILogger<ProductDeletedEventHandler> _logger;

        #endregion

        #region Constructors

        public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task Handle(DeletedEvent<Product> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }

        #endregion
    }
}