using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    public class CreateCampaignCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateCampaignCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
        {
            var product = new Campaign(command.Name, command.Description);
            await context.Campaigns.AddAsync(product);
            await context.SaveChangesAsync();
            
            return product.Id;
        }
    }
}
