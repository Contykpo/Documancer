using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    public record CreateCampaignCommand(string Name, string Description) : IRequest<Guid>;
}