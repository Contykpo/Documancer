using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Update
{
    public record UpdateCampaignCommand(Guid Id, string Name, string Description) : IRequest<bool>;
}
