namespace Application.Services.CampaignServices
{
    public interface IGPTNarratorService
    {
        Task<string> SendMessageAsync(Guid narratorId, string conversationId, string prompt, string model);
        Task<string> StartNewConversationAsync(Guid campaignId, string initialPrompt, string model);
        Task<List<string>> GetConversationHistoryAsync(string conversationId);
    }
}
