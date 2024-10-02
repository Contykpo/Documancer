using Application.Features.CampaignFeatures.DataTransferObjects;
using MediatR;

namespace Application.Features.CampaignFeatures.Queries.Get
{
    public record GetCampaignByIdQuery(Guid Id) : IRequest<CampaignDTO>;
}
