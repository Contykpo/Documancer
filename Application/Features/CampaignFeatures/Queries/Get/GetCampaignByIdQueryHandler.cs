using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Interfaces;
using MediatR;

namespace Application.Features.CampaignFeatures.Queries.Get
{
    public class GetCampaignByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCampaignByIdQuery, CampaignDTO?>
    {
        public async Task<CampaignDTO?> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await context.Campaigns.FindAsync(request.Id);
            
            if (product == null) return null;

            return new CampaignDTO(product.Id, product.Name, product.Description);
        }
    }
}
