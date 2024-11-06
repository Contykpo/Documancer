using Application.Features.NarratorFeatures.DataTransferObjects;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Domain.Entities.Campaigns;
using Infrastructure.Context;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class GPTNarratorRepository(IApplicationDbContext context, IConfiguration configuration) : IGPTNarratorRepository
    {
        public async Task CreateNewConversationAsync(string conversationId, Guid campaignId)
        {
            var conversationNarrator = new Narrator(conversationId, DateTime.UtcNow, campaignId);
            
            context.Narrators.Add(conversationNarrator);
            
            await context.SaveChangesAsync();
        }

        public async Task SaveMessageAsync(Guid ownerNarratorId, string conversationId, string role, string content)
        {
            var message = new NarratorMessage(ownerNarratorId, conversationId, role, content, DateTime.UtcNow);

            context.NarratorMessages.Add(message);

            await context.SaveChangesAsync();
        }

        public async Task<List<NarratorMessageDTO>> GetMessagesByConversationIdAsync(string conversationId)
        {
            var messages = context.NarratorMessages.Where(m => m.ConversationId == conversationId).OrderBy(m => m.Timestamp).ToList();
            var messageDTOs = new List<NarratorMessageDTO>();

            foreach (var message in messages)
            {
                messageDTOs.Add(new NarratorMessageDTO
                {
                    Id = message.Id,
                    OwnerNarratorId = message.OwnerNarratorId,
                    ConversationId = message.ConversationId,
                    Role = message.Role,
                    Content = message.Content,
                    Timestamp = message.Timestamp,
                });
            }

            return messageDTOs;
        }
    }
}
