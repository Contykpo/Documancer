using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.CampaignFeatures.Responses;
using MediatR;

namespace Application.Features.CampaignFeatures.Queries.Get
{
    public record GetCampaignByIdQuery(Guid Id) : IRequest<GetCampaignByIdResponse>;
}
