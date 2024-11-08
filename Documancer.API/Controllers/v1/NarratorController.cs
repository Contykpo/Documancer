using Application.Features.NarratorFeatures.DataTransferObjects;
using Application.Services.CampaignServices;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Documancer.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NarratorController : BaseAPIController
    {
        #region Fields

        private readonly IGPTNarratorService _GPTNarratorService;

        #endregion

        #region Constructors

        public NarratorController(IGPTNarratorService chatGPTService)
        {
            _GPTNarratorService = chatGPTService;
        }

        #endregion

        #region Methods

        [HttpPost("new-conversation")]
        public async Task<IActionResult> StartNewConversation(NarratorMessageDTO request)
        {
            var response = await _GPTNarratorService.StartNewConversationAsync(request.OwnerCampaignId, request.OwnerNarratorId, request.Content, request.Model);
            return Ok(response);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(NarratorMessageDTO request)
        {
            var response = await _GPTNarratorService.SendMessageAsync(request.OwnerNarratorId, request.ConversationId, request.Content, request.Model);
            return Ok(response);
        }

        [HttpGet("get-history/{conversationId}")]
        public async Task<IActionResult> GetConversationHistory(string conversationId)
        {
            var history = await _GPTNarratorService.GetConversationHistoryAsync(conversationId);
            return Ok(history);
        }

        #endregion
    }
}
