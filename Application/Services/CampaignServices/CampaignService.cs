using Application.Features.CampaignFeatures.Responses;
using Application.Features.CampaignFeatures.DataTransferObjects;
using System.Net.Http.Json;

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
