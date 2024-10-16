using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CampaignFeatures.Queries.List
{
    public class ListCampaignsQueryHandler(IApplicationDbContext context) : IRequestHandler<ListCampaignsQuery, List<CampaignDTO>>
    {
        public async Task<List<CampaignDTO>> Handle(ListCampaignsQuery request, CancellationToken cancellationToken)
        {
            return await context.Campaigns
                .Select(c => new CampaignDTO(c.Id, c.Name, c.Description, c.BannerImage))
                .ToListAsync();
        }
    }
}
