using System.Text;
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
                .Select(c => new CampaignDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    OwnerEmailAddress = c.OwnerUser.Email!,
                    FileName = c.BannerImage!.FileName,
                    ContentType = c.BannerImage!.ContentType,
                    Data = c.BannerImage!.Data!
                })
                .ToListAsync();
        }
    }
}