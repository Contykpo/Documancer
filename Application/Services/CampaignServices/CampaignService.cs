using Application.Features.CampaignFeatures.Responses;
using Application.Features.CampaignFeatures.DataTransferObjects;
using System.Net.Http.Json;
using Application.Features.CampaignFeatures.Queries.Get;
using Application.Features.SessionFeatures.Responses;
using Application.Features.SessionFeatures.DataTransferObjects;

namespace Application.Services.CampaignServices
{
    public class CampaignService : ICampaignService
    {
        #region Fields

        private readonly HttpClient httpClient;

        #endregion

        #region Constructor

        public CampaignService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #endregion

        #region Methods

        public async Task<CreateCampaignResponse> CreateCampaignAsync(CampaignDTO campaignDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/v1/campaign/create", campaignDTO);
            var result = await response.Content.ReadFromJsonAsync<CreateCampaignResponse>();

            string error = CheckResponseStatus(response);

            if (!string.IsNullOrEmpty(error)) { return new CreateCampaignResponse(Flag: false, Message: error); }

            return result!;
        }

        public async Task<CreateCampaignSessionResponse> CreateCampaignSessionAsync(SessionDTO sessionDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/v1/session/create", sessionDTO);
            var result = await response.Content.ReadFromJsonAsync<CreateCampaignSessionResponse>();

            string error = CheckResponseStatus(response);

            if (!string.IsNullOrEmpty(error)) { return new CreateCampaignSessionResponse(Flag: false, Message: error); }

            return result!;
        }

        public async Task<GetCampaignByIdResponse> GetCampaignAsync(Guid id)
        {
            var response = await httpClient.GetFromJsonAsync<GetCampaignByIdResponse>($"api/v1/campaign/{id}");

            return response!;
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
