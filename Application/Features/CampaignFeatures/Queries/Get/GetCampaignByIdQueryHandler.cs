using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Interfaces;
using MediatR;

namespace Application.Features.CampaignFeatures.Queries.Get
{
    public class GetCampaignByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCampaignByIdQuery, CampaignDTO?>
    {
        public async Task<CampaignDTO?> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
        {
            var campaign = await context.Campaigns.FindAsync(request.Id);
            
            if (campaign == null) return null;

            return new CampaignDTO(campaign.Id, campaign.Name, campaign.Description, campaign.BannerImage);
        }
    }
}
