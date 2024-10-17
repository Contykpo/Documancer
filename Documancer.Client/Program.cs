using Documancer.Client;
using Application.Services.AuthenticationServices;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Documancer.Client.States;
using Documancer.Client.Extensions;
using Application.Services.CampaignServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();

// Register HttpClient pointing to the backend API with HTTPS.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7103/") });

builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
 
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddMudServices();
builder.Services.AddLocalization();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var host = builder.Build();

await host.SetDefaultCulture();
await host.RunAsync();
