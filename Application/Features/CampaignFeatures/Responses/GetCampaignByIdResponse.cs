using Application.Features.CampaignFeatures.DataTransferObjects;

namespace Application.Features.CampaignFeatures.Responses
{
    public record GetCampaignByIdResponse(bool Flag = false, string Message = null!, CampaignDTO Campaign = null!);
}
