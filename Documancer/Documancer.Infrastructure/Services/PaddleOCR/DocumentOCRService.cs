using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Documancer.Application.Common.Interfaces.Serialization;
using Documancer.Application.Features.Documents.Caching;
using Documancer.Domain.Common.Enums;

namespace Documancer.Infrastructure.Services.PaddleOCR
{
    public class DocumentOCRService : IDocumentOCRService
    {
        #region Fields

        private readonly IApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DocumentOCRService> _logger;
        private readonly IApplicationHubWrapper _notificationService;
        private readonly ISerializer _serializer;
        private readonly Stopwatch _timer;

        #endregion

        #region Constructors

        public DocumentOCRService(
            IApplicationHubWrapper appNotificationService,
            IApplicationDbContext context,
            IHttpClientFactory httpClientFactory,
            ISerializer serializer,
            ILogger<DocumentOCRService> logger)
        {
            _notificationService = appNotificationService;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _serializer = serializer;
            _logger = logger;
            _timer = new Stopwatch();
        }

        #endregion

        #region Methods

        public void Do(int id)
        {
            Recognition(id, CancellationToken.None).Wait();
        }

        public async Task Recognition(int id, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient("ocr"))
                {
                    _timer.Start();
                    
                    var doc = await _context.Documents.FindAsync(id);
                    
                    if (doc == null) return;
                    
                    await _notificationService.JobStarted(id, doc.Title!);
                    
                    DocumentCacheKey.GetOrCreateTokenSource().Cancel();
                    
                    if (string.IsNullOrEmpty(doc.URL)) return;
                    
                    var imgFile = Path.Combine(Directory.GetCurrentDirectory(), doc.URL);
                    
                    if (!File.Exists(imgFile)) return;
                    
                    using var form = new MultipartFormDataContent();
                    using var fileStream = new FileStream(imgFile, FileMode.Open);
                    using var fileContent = new StreamContent(fileStream);

                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");

                    // "image" is the form parameter name for the file
                    form.Add(fileContent, "file", Uri.EscapeDataString(Path.GetFileName(imgFile)));

                    var response = await client.PostAsync("", form);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        
                        if (result.Length > 4000)
                        {
                            result = result.Substring(0, 4000);
                        }
                        
                        doc.Status = JobStatus.Done;
                        doc.Description = "recognize the result: success";
                        doc.Content = result;
                        
                        await _context.SaveChangesAsync(cancellationToken);
                        await _notificationService.JobCompleted(id, doc.Title!);
                        
                        DocumentCacheKey.GetOrCreateTokenSource().Cancel();
                        
                        _timer.Stop();
                        
                        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
                        
                        _logger.LogInformation(
                            "Image recognition completed. Id: {id}, Elapsed Time: {elapsedMilliseconds}ms, Status: {StatusCode}",
                            id, elapsedMilliseconds, response.StatusCode);
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        
                        doc.Status = JobStatus.Pending;
                        doc.Content = result;
                        
                        await _context.SaveChangesAsync(cancellationToken);
                        
                        DocumentCacheKey.GetOrCreateTokenSource().Cancel();
                        
                        await _notificationService.JobCompleted(id, $"Error: {result}");
                        
                        _logger.LogError("{id}: Image recognize error {Message}", id, result);
                    }
                }
            }
            catch (Exception ex)
            {
                await _notificationService.JobCompleted(id, $"Error: {ex.Message}");
                
                _logger.LogError(ex, "{id}: Image recognize error {Message}", id, ex.Message);
            }
        }

        #endregion
    }
#pragma warning disable CS8981
    internal class OcrResult
    {
        [JsonPropertyName("resultcode")] public string? ResultCode { get; set; }

        [JsonPropertyName("message")] public string? Message { get; set; }

        [JsonPropertyName("data")] public List<List<List<dynamic>>>? Data { get; set; }
    }
#pragma warning restore CS8981
}