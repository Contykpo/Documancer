using Application.Features.CampaignFeatures.Commands.Create;
using FluentValidation;

namespace Application.Features.CampaignFeatures.Commands.Update
{
    public class UpdateCampaignCommandValidator : AbstractValidator<UpdateCampaignCommand>
    {
        public UpdateCampaignCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(1);
        }
    }
}
