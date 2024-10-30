using MediatR;

namespace Application.Features.SessionFeatures.Commands.Create
{
    public record CreateSessionCommand(Guid OwnerCampaignId, DateTimeOffset CreationDate, List<string> Notes = null!) : IRequest<Guid>;
}
