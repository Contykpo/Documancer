using Application.Features.CampaignFeatures.DataTransferObjects;

namespace Application.Features.AuthenticationFeatures.Responses
{
    public record GetUserDataResponse(bool Flag = false, string Message = null!, List<CampaignDTO> Campaigns = null!);
}
