using Application.Interfaces;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Delete
{
    public class DeleteCampaignByIdCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteCampaignByIdCommand, bool>
    {
        public async Task<bool> Handle(DeleteCampaignByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Campaigns.FindAsync(request.Id);
            
            if (product == null) return false;
            
            context.Campaigns.Remove(product);
            
            await context.SaveChangesAsync();

            return true;
        }
    }
}
