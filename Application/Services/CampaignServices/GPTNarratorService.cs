using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Features.NarratorFeatures.Responses;
using Application.Interfaces.Contracts;
using Microsoft.Extensions.Configuration;

namespace Application.Services.CampaignServices
{
    public class GPTNarratorService : IGPTNarratorService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IGPTNarratorRepository _conversationRepository;

        public GPTNarratorService(HttpClient httpClient, IConfiguration config, IGPTNarratorRepository conversationRepository)
        {
            _httpClient = httpClient;
            _config = config;
            _conversationRepository = conversationRepository;

            // Set up authorization header for the OpenAI API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["OpenAI:ApiKey"]);
        }

        // Send a message in an existing conversation
        public async Task<string> SendMessageAsync(Guid narratorId, string conversationId, string prompt, string model = "gpt-3.5-turbo")
        {
            var requestBody = new
            {
                model = model,
                messages = new[] { new { role = "user", content = prompt } }
            };

            var response = await _httpClient.PostAsJsonAsync($"{_config["OpenAI:BaseUrl"]}/chat/completions", requestBody);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OpenAINarratorResponse>();
            var aiResponse = result?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response";

            // Save the conversation message to the database
            await _conversationRepository.SaveMessageAsync(narratorId, conversationId, "user", prompt);
            await _conversationRepository.SaveMessageAsync(narratorId, conversationId, "assistant", aiResponse);

            return aiResponse;
        }

        // Start a new conversation with an initial prompt
        public async Task<string> StartNewConversationAsync(Guid campaignId, string initialPrompt, string model = "gpt-3.5-turbo")
        {
            // Generate a unique conversation ID (could be GUID or similar)
            var conversationId = Guid.NewGuid().ToString();

            // Create an initial message request to start the conversation
            var requestBody = new
            {
                model = model,
                messages = new[] { new { role = "user", content = initialPrompt } }
            };

            var response = await _httpClient.PostAsJsonAsync($"{_config["OpenAI:BaseUrl"]}/chat/completions", requestBody);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OpenAINarratorResponse>();
            var aiResponse = result?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response";

            // Save the initial messages to the database under the new conversation ID:
            var narratorId = await _conversationRepository.CreateNewConversationAsync(conversationId, campaignId);
            
            await _conversationRepository.SaveMessageAsync(narratorId, conversationId, "user", initialPrompt);
            await _conversationRepository.SaveMessageAsync(narratorId, conversationId, "assistant", aiResponse);

            return aiResponse;
        }

        // Fetch conversation history for a given conversation ID
        public async Task<List<string>> GetConversationHistoryAsync(string conversationId)
        {
            var messages = await _conversationRepository.GetMessagesByConversationIdAsync(conversationId);

            // Format messages for display, e.g., "Role: MessageContent"
            return messages.Select(m => $"{m.Role}: {m.Content}").ToList();
        }
    }
}
