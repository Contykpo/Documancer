namespace Documancer.Application.Features.Contacts.EventHandlers
{
    public class ContactCreatedEventHandler : INotificationHandler<ContactCreatedEvent>
    {
        #region Properties and Fields

        private readonly ILogger<ContactCreatedEventHandler> _logger;

        #endregion

        #region Constructors

        public ContactCreatedEventHandler(ILogger<ContactCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task Handle(ContactCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }

        #endregion
    }
}