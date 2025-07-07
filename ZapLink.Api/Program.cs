using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// ✅ Register CosmosClient
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    string connStr = config["COSMOSDB_CONNECTION"];
    return new CosmosClient(connStr);
});

builder.Build().Run();
