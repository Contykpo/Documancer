using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.CampaignFeatures.Responses;

namespace Application.Services.CampaignServices
{
    public interface ICampaignService
    {
        Task<CreateCampaignResponse> CreateCampaignAsync(CampaignDTO campaignDTO);
        Task<GetCampaignByIdResponse> GetCampaignAsync(Guid campaignDTO);
    }
}
