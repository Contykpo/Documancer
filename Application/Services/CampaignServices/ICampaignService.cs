using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.CampaignFeatures.Responses;

namespace Application.Services.CampaignServices
{
    public interface ICampaignService
    {
        Task<CreateCampaignResponse> CreateCampaignAsync(CampaignDTO campaignDTO);
    }
}
