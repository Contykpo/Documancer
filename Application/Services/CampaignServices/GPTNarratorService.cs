using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using Application.Features.NarratorFeatures.DataTransferObjects;
using Application.Features.NarratorFeatures.Responses;
using Application.Interfaces.Contracts;
using Domain.Entities.Campaigns;
using Microsoft.Extensions.Configuration;

namespace Application.Services.CampaignServices
{
    public class GPTNarratorService : IGPTNarratorService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly IConfiguration _config;
        private readonly IGPTNarratorRepository _conversationRepository;

        #endregion

        #region Constructor

        public GPTNarratorService(HttpClient httpClient, IConfiguration config, IGPTNarratorRepository conversationRepository)
        {
            this.httpClient = httpClient;
            _config = config;
            _conversationRepository = conversationRepository;

            // Set up authorization header for the OpenAI API.
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["OpenAI:ApiKey"]);
        }

        #endregion

        #region Methods

        // Send a message in an existing conversation.
        public async Task<SendMessageResponse> SendMessageAsync(Guid narratorId, string conversationId, string prompt, string model = "gpt-3.5-turbo")
        {
            var requestBody = new
            {
                model = model,
                messages = new[] { new { role = "user", content = prompt } }
            };

            var openAIResponse = await httpClient.PostAsJsonAsync($"{_config["OpenAI:BaseUrl"]}/chat/completions", requestBody);
            openAIResponse.EnsureSuccessStatusCode();

            var aiResult = await openAIResponse.Content.ReadFromJsonAsync<OpenAINarratorResponse>();
            var aiResponse = aiResult?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response";

            var narratorMessage = new NarratorMessageDTO()
            {
                OwnerNarratorId = narratorId,
                ConversationId = conversationId,
                Role = "user",
                Content = aiResponse,
                Model = model,
                Timestamp = DateTime.UtcNow
            };

            var response = await httpClient.PostAsJsonAsync("api/v1/narrator/send-message", narratorMessage);
            var result = await response.Content.ReadFromJsonAsync<SendMessageResponse>();

            string error = CheckResponseStatus(response);

            if (!string.IsNullOrEmpty(error)) { return new SendMessageResponse(Flag: false, Message: error); }

            return result!;
        }

        // Start a new conversation with an initial prompt.
        public async Task<CreateNewConversationResponse> StartNewConversationAsync(Guid campaignId, string initialPrompt, string model = "gpt-3.5-turbo")
        {
            // Generate a unique conversation ID (could be GUID or similar).
            var conversationId = Guid.NewGuid().ToString();

            // Create an initial message request to start the conversation.
            var requestBody = new
            {
                model = model,
                messages = new[] { new { role = "user", content = initialPrompt } }
            };

            var openAIResponse = await httpClient.PostAsJsonAsync($"{_config["OpenAI:BaseUrl"]}/chat/completions", requestBody);
            openAIResponse.EnsureSuccessStatusCode();

            var aiResult = await openAIResponse.Content.ReadFromJsonAsync<OpenAINarratorResponse>();
            var aiResponse = aiResult?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response";

            var narratorMessage = new NarratorMessageDTO()
            {
                OwnerCampaignId = campaignId,
                ConversationId = conversationId,
                Role = "user",
                Content = aiResponse,
                Model = model,
                Timestamp = DateTime.UtcNow
            };

            var response = await httpClient.PostAsJsonAsync("api/v1/narrator/new-conversation", narratorMessage);
            var result = await response.Content.ReadFromJsonAsync<CreateNewConversationResponse>();

            string error = CheckResponseStatus(response);

            if (!string.IsNullOrEmpty(error)) { return new CreateNewConversationResponse(Flag: false, Message: error); }

            return result!;
        }

        // Fetch conversation history for a given conversation ID.
        public async Task<List<string>> GetConversationHistoryAsync(string conversationId)
        {
            var messages = await _conversationRepository.GetMessagesByConversationIdAsync(conversationId);

            // Format messages for display, e.g., "Role: MessageContent".
            return messages.Select(m => $"{m.Role}: {m.Content}").ToList();
        }


        // Class-specific methods:

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return $"Sorry unkown error occured.{Environment.NewLine}Error Description:{Environment.NewLine}Status Code: {response.StatusCode}{Environment.NewLine}Reason Phrase: {response.ReasonPhrase}";
            }
            else
            {
                return null!;
            }
        }

        #endregion
    }
}
