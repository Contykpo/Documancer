using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.CampaignFeatures.Responses;
using Application.Features.SessionFeatures.DataTransferObjects;
using Application.Features.SessionFeatures.Responses;

namespace Application.Services.CampaignServices
{
    public interface ICampaignService
    {
        Task<CreateCampaignResponse> CreateCampaignAsync(CampaignDTO campaignDTO);
        Task<CreateCampaignSessionResponse> CreateCampaignSessionAsync(SessionDTO sessionDTO);
        Task<GetCampaignByIdResponse> GetCampaignAsync(Guid campaignDTO);
    }
}
