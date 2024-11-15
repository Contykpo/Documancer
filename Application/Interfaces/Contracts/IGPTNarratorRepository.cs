using Application.Features.NarratorFeatures.DataTransferObjects;
using Application.Features.NarratorFeatures.Responses;

namespace Application.Interfaces.Contracts
{
    public interface IGPTNarratorRepository
    {
        Task<CreateNewConversationResponse> CreateNewConversationAsync(string conversationId, Guid campaignId);
        Task<SendMessageResponse> SaveMessageAsync(Guid ownerNarratorId, string conversationId, string role, string content);
        Task<List<NarratorMessageDTO>> GetMessagesByConversationIdAsync(string conversationId);
    }
}
