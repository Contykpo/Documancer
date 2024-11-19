using Application.Features.AuthenticationFeatures.Responses;
using Application.Features.NarratorFeatures.DataTransferObjects;
using Application.Features.NarratorFeatures.Responses;
using Application.Interfaces.Contracts;
using Application.Services.CampaignServices;
using Asp.Versioning;
using Domain.Entities.Campaigns;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NarratorController : BaseAPIController
    {
        #region Fields

        private readonly IGPTNarratorRepository _GPTNarratorRepository;

        #endregion

        #region Constructors

        public NarratorController(IGPTNarratorRepository gptNarratorRepository)
        {
            _GPTNarratorRepository = gptNarratorRepository;
        }

        #endregion

        #region Methods

        [HttpPost("new-conversation")]
        public async Task<ActionResult<CreateNewConversationResponse>> StartNewConversation(NarratorMessageDTO request)
        {
            // Save the initial messages to the database under the new conversation ID:
            var narratorId = await _GPTNarratorRepository.CreateNewConversationAsync(request.ConversationId, request.OwnerCampaignId);

            await _GPTNarratorRepository.SaveMessageAsync(request.OwnerNarratorId, request.ConversationId, "user", request.AssociatedPrompt);
            await _GPTNarratorRepository.SaveMessageAsync(request.OwnerNarratorId, request.ConversationId, "assistant", request.Content);

            return Ok(narratorId);
        }

        [HttpPost("send-message")]
        public async Task<ActionResult<SendMessageResponse>> SendMessage(NarratorMessageDTO request)
        {
            // Save the conversation message to the database.
            await _GPTNarratorRepository.SaveMessageAsync(request.OwnerNarratorId, request.ConversationId, "user", request.AssociatedPrompt);

            return Ok(await _GPTNarratorRepository.SaveMessageAsync(request.OwnerNarratorId, request.ConversationId, "assistant", request.Content));
        }

        [HttpGet("get-history/{conversationId}")]
        public async Task<ActionResult<GetMessagesByConversationIdResponse>> GetConversationHistory(string conversationId)
        {
            return Ok(await _GPTNarratorRepository.GetMessagesByConversationIdAsync(conversationId));
        }

        #endregion
    }
}
