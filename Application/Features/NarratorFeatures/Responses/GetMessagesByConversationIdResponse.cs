using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.NarratorFeatures.DataTransferObjects;

namespace Application.Features.NarratorFeatures.Responses
{
    public record GetMessagesByConversationIdResponse(bool Flag = false, string Message = null!, List<NarratorMessageDTO> Messages = null!);
}
