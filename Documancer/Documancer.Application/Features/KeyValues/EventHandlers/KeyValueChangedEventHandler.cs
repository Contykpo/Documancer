namespace Documancer.Application.Features.KeyValues.EventHandlers;

public class KeyValueChangedEventHandler : INotificationHandler<UpdatedEvent<KeyValue>>
{
    #region Properties and Fields

    private readonly ILogger<KeyValueChangedEventHandler> _logger;
    private readonly IPicklistService _picklistService;

    #endregion

    #region Constructors

    public KeyValueChangedEventHandler(IPicklistService picklistService, ILogger<KeyValueChangedEventHandler> logger)
    {
        _picklistService = picklistService;
        _logger = logger;
    }

    #endregion

    #region Methods

    public Task Handle(UpdatedEvent<KeyValue> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("KeyValue Changed {DomainEvent},{@Entity}", nameof(notification), notification.Entity);
        _picklistService.Refresh();
        
        return Task.CompletedTask;
    }

    #endregion
}