using Documancer.Application.DataTransferObjects.Request.Account;
using Documancer.Application.DataTransferObjects.Response;
using Documancer.Application.DataTransferObjects.Response.Account;
using Documancer.Application.Extensions;
using Documancer.Application.Extensions.Services;
using System.Net.Http.Json;

namespace Documancer.Application.Services
{
    public class AccountService (HttpClientService httpClientService) : IAccountServices
    {
        // Interface methods:

        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            try
            {
                var publicClient = await httpClientService.GetPrivateClient();
                var response = await publicClient.PostAsJsonAsync(Constant.ChangeUserRoleRoute, model);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error)) { return new GeneralResponse(false, error); }

                var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();

                return result!;
            }
            catch (Exception exception)
            {
                return new GeneralResponse(false, exception.Message);
            }
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.RegisterRoute, model);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error)) { return new GeneralResponse(Flag: false, Message: error); }

                var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();

                return result;
            }
            catch (Exception exception)
            {
                return new GeneralResponse(Flag: false, Message: exception.Message);
            }
        }

        public async Task CreateAdmin()
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsync(Constant.CreateAdminRoute, null);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to create admin: {exception.Message}");
            }
        }

        public async Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.CreateRoleRoute, model);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error))
                {
                    return new GeneralResponse(false, error);
                }

                var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();

                return result ?? new GeneralResponse(false, "Failed to deserialize the response.");
            }
            catch (Exception exception)
            {
                return new GeneralResponse(false, exception.Message);
            }
        }

        public async Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync()
        {
            try
            {
                var privateClient = await httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync(Constant.GetUsersWithRolesRoute);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error)) { throw new Exception(error); }

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetUsersWithRolesResponseDTO>>();

                return result!;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.LoginRoute, model);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error)) { return new LoginResponse(Flag: false, Message: error); }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                return result;
            }
            catch (Exception exception)
            {
                return new LoginResponse(Flag: false, Message: exception.Message);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.RefreshTokenRoute, model);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error)) { return new LoginResponse(false, error); }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                return result!;
            }
            catch (Exception exception)
            {
                return new LoginResponse(false, exception.Message);
            }
        }


        // Classic-specific methods:

        public async Task CreateAdminAtFirstStart()
        {
            try
            {
                var client = httpClientService.GetPublicClient();

                await client.PostAsync(Constant.CreateAdminRoute, null);
            }
            catch { }
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
        {
            try
            {
                var privateClient = await httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync(Constant.GetRolesRoute);

                string error = CheckResponseStatus(response);

                if (!string.IsNullOrEmpty(error)) { throw new Exception(error); }

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetRoleDTO>>();

                return result!;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        //public IEquatable<GetRoleDTO> GetDefaultRoles()
        //{
        //    var list = new List<GetRoleDTO>();

        //    list?.Clear();
        //    list.Add(new GetRoleDTO(1.ToString(), Constant.Role.Admin));
        //    list.Add(new GetRoleDTO(2.ToString(), Constant.Role.User));

        //    return list;
        //}

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return $"Sorry unkown error occured.{Environment.NewLine}Error Description:{Environment.NewLine}Status Code: {response.StatusCode}{Environment.NewLine}Reason Phrase: {response.ReasonPhrase}";
            }
            else
            {
                return null;
            }
        }
    }
}
