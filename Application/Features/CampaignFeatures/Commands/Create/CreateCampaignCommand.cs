using Domain.Entities.Files;
using MediatR;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    public record CreateCampaignCommand(string Name, string Description, Image? BannerImage) : IRequest<Guid>;
}