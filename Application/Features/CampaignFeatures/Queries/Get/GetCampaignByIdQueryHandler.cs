using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.CampaignFeatures.Responses;
using Application.Features.SessionFeatures.DataTransferObjects;
using Application.Interfaces;
using Domain.Entities.Campaigns;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CampaignFeatures.Queries.Get
{
    public class GetCampaignByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCampaignByIdQuery, GetCampaignByIdResponse?>
    {
        public async Task<GetCampaignByIdResponse?> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var campaign = await context.Campaigns.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (campaign == null) return new GetCampaignByIdResponse(false, "Failed to retrieve Campaign.");

                var ownerUser = await context.Users.FirstOrDefaultAsync(u => u.Id == campaign.OwnerUserId);                
                if (ownerUser == null) return new GetCampaignByIdResponse(false, "Failed to retrieve Campaign Owner's email address.");

                var campaignSessions = await context.Sessions.Where(s => s.OwnerCampaignId == campaign.Id).ToListAsync();
                
                var bannerImage = await context.Images.FirstOrDefaultAsync(i => i.OwnerCampaignId == campaign.Id);

                var campaignDTO = new CampaignDTO
                {
                    Id = campaign.Id,
                    Name = campaign.Name,
                    Description = campaign.Description,

                    OwnerEmailAddress = ownerUser!.Email!,

                    FileName = bannerImage != null ? bannerImage.FileName : string.Empty,
                    ContentType = bannerImage != null ? bannerImage.ContentType : string.Empty,
                    Data = bannerImage != null ? bannerImage.Data! : []
                };

                foreach (var session in campaignSessions)
                {
                    campaignDTO.Sessions.Add(new SessionDTO
                    {
                        Id = session.Id,
                        OwnerCampaignId = (Guid)session!.OwnerCampaignId!,
                        CreationDate = session.CreationDate,
                        Notes = new List<string>(session.Notes)
                    });
                }

                return new GetCampaignByIdResponse(true, $"Successfully retrieved Campaign with Id: {campaignDTO.Id}.", campaignDTO);
            }
            catch (Exception exception)
            {
                return new GetCampaignByIdResponse(false, exception.Message);
            }
        }
    }
}