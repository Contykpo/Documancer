using FluentValidation;

namespace Application.Features.CampaignFeatures.Commands.Create
{
    public class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommand>
    {
        public CreateCampaignCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(1);
        }
    }
}