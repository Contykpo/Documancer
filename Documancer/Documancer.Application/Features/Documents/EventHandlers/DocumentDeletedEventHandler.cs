namespace Documancer.Application.Features.Documents.EventHandlers;

public class DocumentDeletedEventHandler : INotificationHandler<DeletedEvent<Document>>
{
    #region Properties and Fields

    private readonly ILogger<DocumentDeletedEventHandler> _logger;

    #endregion

    #region Constructors

    public DocumentDeletedEventHandler(ILogger<DocumentDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Methods

    public Task Handle(DeletedEvent<Document> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete file: {FileName}", notification.Entity.URL);
        
        if (string.IsNullOrEmpty(notification.Entity.URL))
        {
            return Task.CompletedTask;
        }

        var folder = UploadType.Document.GetDescription();
        var folderName = Path.Combine("Files", folder);
        var deleteFile = Path.Combine(Directory.GetCurrentDirectory(), folderName, notification.Entity.URL);
        
        if (File.Exists(deleteFile))
        {
            File.Delete(deleteFile);
        }

        return Task.CompletedTask;
    }

    #endregion
}