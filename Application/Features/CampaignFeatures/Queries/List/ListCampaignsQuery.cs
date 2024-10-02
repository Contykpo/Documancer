using Application.Features.CampaignFeatures.DataTransferObjects;
using MediatR;

namespace Application.Features.CampaignFeatures.Queries.List
{
    public record ListCampaignsQuery : IRequest<List<CampaignDTO>>;
}
