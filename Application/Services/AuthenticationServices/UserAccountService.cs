using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using System.Net.Http.Json;

namespace Application.Services.AuthenticationServices
{
    public class UserAccountService : IUserAccountService
    {
        private readonly HttpClient httpClient;

        public UserAccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<LoginResponse> LogInUserAccountAsync(LoginUserDTO loginUserDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/v1/user/login", loginUserDTO);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            string error = CheckResponseStatus(response);

            if (!string.IsNullOrEmpty(error)) { return new LoginResponse(Flag: false, Message: error); }

            return result!;
        }

        public async Task<RegistrationResponse> RegisterUserAccountAsync(RegisterUserDTO registerUserDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/v1/user/register", registerUserDTO);
            var result = await response.Content.ReadFromJsonAsync<RegistrationResponse>();

            string error = CheckResponseStatus(response);

            if (!string.IsNullOrEmpty(error)) { return new RegistrationResponse(Flag: false, Message: error); }

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
    }
}
