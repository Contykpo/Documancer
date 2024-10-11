using Application.Interfaces;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Update
{
    public class UpdateCampaignCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateCampaignCommand, bool>
    {
        public async Task<bool> Handle(UpdateCampaignCommand command, CancellationToken cancellationToken)
        {
            var campaign = await context.Campaigns.FindAsync(command.Id);

            if (campaign == null)
            {
                return false;
            }

            campaign.Name = command.Name;
            campaign.Description = command.Description;
            
            if (command.BannerImage is not null)
            {
                campaign.BannerImage = command.BannerImage;
            }

            await context.SaveChangesAsync();

            return true;
        }
    }

}
