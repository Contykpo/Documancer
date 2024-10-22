using System.Text;
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

            var campaign = new Campaign(command.Name, command.Description, context.Users.FirstOrDefault(u => u.Email == command.OwnerEmailAddress)!, bannerImage);

            if (command.Data != string.Empty &&
                command.FileName != string.Empty &&
                command.ContentType != string.Empty)
            {
                bannerImage = new Image(command.FileName, command.ContentType, Encoding.ASCII.GetBytes(command.Data), campaign);

                campaign.BannerImage = bannerImage;

                await context.Images.AddAsync(bannerImage, cancellationToken);
            }

            await context.Campaigns.AddAsync(campaign, cancellationToken);

            context.Users.FirstOrDefault(u => u.Email == command.OwnerEmailAddress)!.Campaigns.Add(campaign);

            await context.SaveChangesAsync();

            return campaign.Id;
        }
    }
}