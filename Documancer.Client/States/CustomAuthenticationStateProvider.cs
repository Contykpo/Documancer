using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Documancer.Client.States
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region Events

        public event Action<ClaimsPrincipal>? UserChanged;

        #endregion

        #region Properties and Fields

        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        private readonly ILocalStorageService localStorageService;
        
        private const string localStorageKey = "auth";

        #endregion

        #region Constructors

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        #endregion

        #region Methods

        // Abstract class implementation:

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await localStorageService.GetItemAsStringAsync(localStorageKey)!;

            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            var (name, email) = GetClaims(token);

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            var claims = SetClaimPrincipal(name, email);

            if (claims is null)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            else
            {
                return await Task.FromResult(new AuthenticationState(claims));
            }
        }


        // Class-specific methods:

        public static ClaimsPrincipal SetClaimPrincipal(string name, string email)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email)) return new ClaimsPrincipal();

            return new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new(ClaimTypes.Name, name!),
                    new(ClaimTypes.Email, email!)
                ],
                "JwtAuth"));
        }

        private static (string, string) GetClaims(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken)) return (null!, null!);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)!.Value;
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email)!.Value;

            return (name, email);
        }

        public async Task UpdateAuthenticationState(string jwtToken)
        {
            var claims = new ClaimsPrincipal();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                var (name, email) = GetClaims(jwtToken);

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email)) return;

                var setClaims = SetClaimPrincipal(name, email);

                if (setClaims is null) return;

                await localStorageService.SetItemAsStringAsync(localStorageKey, jwtToken);
            }
            else
            {
                await localStorageService.RemoveItemAsync(localStorageKey);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        }

        #endregion
    }
}
