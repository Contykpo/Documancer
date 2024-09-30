using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Documancer.Application.Features.Documents.EventHandlers;

public class DocumentCreatedEventHandler : INotificationHandler<CreatedEvent<Document>>
{
    #region Properties and Fields

    private readonly ILogger<DocumentCreatedEventHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    #endregion

    #region Constructors

    public DocumentCreatedEventHandler(
        IServiceScopeFactory scopeFactory,
        ILogger<DocumentCreatedEventHandler> logger
    )
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    #endregion

    #region Methods

    public Task Handle(CreatedEvent<Document> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Document upload successful. Beginning document recognition process for Document Id: {DocumentId}", notification.Entity.Id);
        
        var domainEvent = notification.Entity;
        var id = domainEvent.Id;
        var ocrJob = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IDocumentOCRService>();
        
        BackgroundJob.Enqueue(() => ocrJob.Do(id));
        
        return Task.CompletedTask;
    }

    #endregion
}