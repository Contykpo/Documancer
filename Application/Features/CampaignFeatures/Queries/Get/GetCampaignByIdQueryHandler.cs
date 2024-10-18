using System.Text;
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

            var campaignDTO = new CampaignDTO
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description,

                OwnerEmailAddress = campaign.OwnerUser.Email!,

                FileName = campaign.BannerImage!.FileName,
                ContentType = campaign.BannerImage.ContentType,
                Data = Encoding.ASCII.GetString(campaign.BannerImage!.Data!) 
            };

            return campaignDTO;

        }
    }
}
