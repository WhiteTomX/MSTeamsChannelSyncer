using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WhiteTom.MsTeamsChannelSyncer.Services;
using WhiteTom.MsTeamsChannelSyncer.Repositories;
using Microsoft.Graph;
using Azure.Identity;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<GraphServiceClient>(_ =>
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            return new GraphServiceClient(new DefaultAzureCredential(), scopes);
        });
        services.AddSingleton<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<IChannelRepository, GraphChannelRepository>();
        services.AddScoped<ISynchronizationService, GraphSynchronizationService>();
    })
    .Build();

host.Run();
