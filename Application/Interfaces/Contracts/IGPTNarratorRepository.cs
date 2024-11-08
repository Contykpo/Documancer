using Application.Features.NarratorFeatures.DataTransferObjects;

namespace Application.Interfaces.Contracts
{
    public interface IGPTNarratorRepository
    {
        Task<Guid> CreateNewConversationAsync(string conversationId, Guid campaignId);
        Task SaveMessageAsync(Guid ownerNarratorId, string conversationId, string role, string content);
        Task<List<NarratorMessageDTO>> GetMessagesByConversationIdAsync(string conversationId);
    }
}
