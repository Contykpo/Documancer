using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Features.NarratorFeatures.DataTransferObjects;
using Application.Features.NarratorFeatures.Responses;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Domain.Entities.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class GPTNarratorRepository(IApplicationDbContext context, IConfiguration configuration) : IGPTNarratorRepository
    {
        public async Task<CreateNewConversationResponse> CreateNewConversationAsync(string conversationId, Guid campaignId)
        {
            try
            {
                var conversationNarrator = new Narrator(conversationId, DateTime.UtcNow, campaignId);

                context.Narrators.Add(conversationNarrator);

                await context.SaveChangesAsync();

                return new CreateNewConversationResponse(true, $"Successfully created new conversation narrator: {conversationNarrator.Id}, {conversationId}", conversationNarrator.Id.ToString());
            }
            catch (Exception exception)
            {
                return new CreateNewConversationResponse(false, exception.Message);
            }
        }

        public async Task<SendMessageResponse> SaveMessageAsync(Guid ownerNarratorId, string conversationId, string role, string content)
        {
            try
            {
                var message = new NarratorMessage(ownerNarratorId, conversationId, role, content, DateTime.UtcNow);

                context.NarratorMessages.Add(message);

                await context.SaveChangesAsync();

                return new SendMessageResponse(true, $"Successfully sent message prompt and saved narrator response: {message.OwnerNarratorId}.");
            }
            catch (Exception exception)
            {
                return new SendMessageResponse(false, exception.Message);
            }
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
