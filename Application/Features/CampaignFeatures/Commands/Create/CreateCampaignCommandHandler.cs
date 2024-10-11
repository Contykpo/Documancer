using Application.Interfaces;
using Domain.Entities.Campaigns;
using Domain.Entities.Files;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    public class CreateCampaignCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateCampaignCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
        {
            Image? bannerImage = null;

            if (command.Data != null &&
                command.FileName != null &&
                command.ContentType != null)
            {
                bannerImage = new Image
                {
                    FileName = command.FileName,
                    ContentType = command.ContentType,
                    Data = command.Data
                };

                await context.Images.AddAsync(bannerImage, cancellationToken);
            }

            var campaign = new Campaign(command.Name, command.Description, bannerImage);
            await context.Campaigns.AddAsync(campaign, cancellationToken);

            await context.SaveChangesAsync();

            return campaign.Id;
        }
    }
}
