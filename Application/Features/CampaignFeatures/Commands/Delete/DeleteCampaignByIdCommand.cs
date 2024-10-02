using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Delete
{
    public record DeleteCampaignByIdCommand(Guid Id) : IRequest<bool>;
}
