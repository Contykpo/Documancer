using Application.Features.NarratorFeatures.Responses;

namespace Application.Services.CampaignServices
{
    public interface IGPTNarratorService
    {
        Task<SendMessageResponse> SendMessageAsync(Guid narratorId, string conversationId, string prompt, string model);
        Task<CreateNewConversationResponse> StartNewConversationAsync(Guid campaignId, string initialPrompt, string model);
        Task<GetMessagesByConversationIdResponse> GetConversationHistoryAsync(string conversationId);
    }
}
