using Application.Interfaces;
using Domain.Entities.Campaigns;
using MediatR;

namespace Application.Features.SessionFeatures.Commands.Create
{
    public class CreateSessionCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateSessionCommand, Guid>
    {
        public async Task<Guid> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
        {
            var session = new Session(command.CreationDate, command.OwnerCampaignId, command.Notes);

            await context.Sessions.AddAsync(session, cancellationToken);

            context.Campaigns.FirstOrDefault(c => c.Id == command.OwnerCampaignId)!.Sessions.Add(session);

            await context.SaveChangesAsync();

            return session.Id;
        }
    }
}
