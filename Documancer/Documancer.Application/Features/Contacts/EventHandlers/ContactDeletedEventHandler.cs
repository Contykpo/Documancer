namespace Documancer.Application.Features.Contacts.EventHandlers
{
    public class ContactDeletedEventHandler : INotificationHandler<ContactDeletedEvent>
    {
        #region Properties and Fields

        private readonly ILogger<ContactDeletedEventHandler> _logger;

        #endregion

        #region Constructors

        public ContactDeletedEventHandler(ILogger<ContactDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task Handle(ContactDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            
            return Task.CompletedTask;
        }

        #endregion
    }
}
