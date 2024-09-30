using System.Diagnostics;

namespace Documancer.Application.Features.Products.EventHandlers
{
    public class ProductCreatedEventHandler : INotificationHandler<CreatedEvent<Product>>
    {
        #region Properties and Fields

        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly Stopwatch _timer;

        #endregion

        #region Constructors

        public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        #endregion

        #region Methods

        public async Task Handle(CreatedEvent<Product> notification, CancellationToken cancellationToken)
        {
            _timer.Start();

            await Task.Delay(5000, cancellationToken);

            _timer.Stop();

            _logger.LogInformation("Domain Event: {DomainEvent},{ElapsedMilliseconds}ms", notification.GetType().FullName, _timer.ElapsedMilliseconds);
        }

        #endregion
    }
}