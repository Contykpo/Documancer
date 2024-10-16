using Domain.Entities.Files;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Update
{
    public record UpdateCampaignCommand(Guid Id, string Name, string Description, string? FileName = null, string? ContentType = null, byte[]? Data = null) : IRequest<bool>;
}
