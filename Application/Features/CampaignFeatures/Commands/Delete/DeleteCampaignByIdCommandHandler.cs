﻿using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CampaignFeatures.Commands.Delete
{
    public class DeleteCampaignByIdCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteCampaignByIdCommand, bool>
    {
        public async Task<bool> Handle(DeleteCampaignByIdCommand request, CancellationToken cancellationToken)
        {
            var campaign = await context.Campaigns.FindAsync(request.Id);
            
            if (campaign == null) return false;

            context.Images.Remove(campaign.BannerImage!);
            
            context.Users.FirstOrDefault(user => user.Id == campaign.OwnerUser.Id)!.Campaigns.Remove(campaign);

            context.Campaigns.Remove(campaign);
            
            await context.SaveChangesAsync();

            return true;
        }
    }
}
