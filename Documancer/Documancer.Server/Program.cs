using Documancer.Application;
using Documancer.Infrastructure;
using Documancer.Server;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterSerilog();
builder.WebHost.UseStaticWebAssets();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddServerUI(builder.Configuration);

var app = builder.Build();

app.ConfigureServer(builder.Configuration);

await app.InitializeDatabaseAsync().ConfigureAwait(false);

await app.RunAsync().ConfigureAwait(false);