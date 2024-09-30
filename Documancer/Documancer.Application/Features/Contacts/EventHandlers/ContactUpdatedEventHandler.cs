namespace Documancer.Application.Features.Contacts.EventHandlers
{
    public class ContactUpdatedEventHandler : INotificationHandler<ContactUpdatedEvent>
    {
        #region Properties and Fields

        private readonly ILogger<ContactUpdatedEventHandler> _logger;

        #endregion

        #region Constructors

        public ContactUpdatedEventHandler(ILogger<ContactUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task Handle(ContactUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);

            return Task.CompletedTask;
        }

        #endregion
    }
}