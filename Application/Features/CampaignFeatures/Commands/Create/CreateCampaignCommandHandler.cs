using Application.Interfaces;
using Domain.Entities.Campaigns;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    public class CreateCampaignCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateCampaignCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
        {
            var campaign = new Campaign(command.Name, command.Description, command.BannerImage);
            
            if (command.BannerImage is not null)
            {
                await context.Images.AddAsync(campaign.BannerImage!);
            }

            await context.Campaigns.AddAsync(campaign);
            await context.SaveChangesAsync();
            
            return campaign.Id;
        }
    }
}
