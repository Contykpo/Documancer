using Documancer.Application.DataTransferObjects.Request.Account;
using Documancer.Application.DataTransferObjects.Response.Account;
using Documancer.Application.Extensions.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Documancer.Application.Extensions
{
    public class CustomAuthenticationStateProvider (LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenModel = await localStorageService.GetModelFromToken();

            if (string.IsNullOrEmpty(tokenModel.Token)) { return await Task.FromResult(new AuthenticationState(anonymous)); }

            var getUserClaims = DecryptToken(tokenModel.Token!);

            if (getUserClaims == null) { return await Task.FromResult(new AuthenticationState(anonymous)); }

            var claimsPrincipal = SetClaimPrincipal(getUserClaims);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task UpdateAuthenticationState(LocalStorageDTO localStorageDTO)
        {
            var claimsPrinciapl = new ClaimsPrincipal();

            if (localStorageDTO.Token != null || localStorageDTO.Refresh != null)
            {
                await localStorageService.SetBrowserLocalStorage(localStorageDTO);

                var getUserClaims = DecryptToken(localStorageDTO.Token!);

                claimsPrinciapl = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveTokenFromBrowserLocalStorage();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrinciapl)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(UserClaimsDTO userClaims)
        {
            if (userClaims.EmailAddress is null) { return new ClaimsPrincipal(); }

            return new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new(ClaimTypes.Name, userClaims.Username!),
                    new(ClaimTypes.Email, userClaims.EmailAddress!),
                    new(ClaimTypes.Role, userClaims.Role!),
                    new("Fullname", userClaims.Fullname!),
                ],
                Constant.AuthenticationType));
        }

        private static UserClaimsDTO DecryptToken(string jwtToken)
        {
            try
            {
                if (string.IsNullOrEmpty(jwtToken)) { return new UserClaimsDTO(); }

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)!.Value;
                var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email)!.Value;
                var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role)!.Value;
                var fullname = token.Claims.FirstOrDefault(_ => _.Type == "Fullname")!.Value;

                return new UserClaimsDTO(fullname, name, email, role);
            }
            catch
            {
                return null;
            }
        }
    }
}
