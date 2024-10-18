using Domain.Entities.Files;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    // TODO: Might want to rework this request in case we want to port to other systems, such as a hybrid application.
    public record CreateCampaignCommand(string Name, string Description, string OwnerEmailAddress, string? FileName = null, string? ContentType = null, string? Data = null) : IRequest<Guid>;
}