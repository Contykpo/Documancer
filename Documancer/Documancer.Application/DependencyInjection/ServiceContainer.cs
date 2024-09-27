using Documancer.Application.Extensions;
using Documancer.Application.Extensions.Services;
using Documancer.Application.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace Documancer.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAccountServices, AccountService>();

            services.AddAuthorizationCore();

            services.AddNetcodeHubLocalStorageService();

            services.AddScoped<Extensions.Services.LocalStorageService>();
            services.AddScoped<HttpClientService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddTransient<CustomHttpHandler>();

            services.AddCascadingAuthenticationState();

            services.AddHttpClient("DocumancerClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7060/");
            }).AddHttpMessageHandler<CustomHttpHandler>();

            return services;
        }
    }
}
