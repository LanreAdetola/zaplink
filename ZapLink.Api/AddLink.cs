using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using ZapLink.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class AddLink
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger _logger;
    private readonly string _databaseId = "ZapLinkDB";
    private readonly string _containerId = "Links";

    public AddLink(CosmosClient cosmosClient, ILoggerFactory loggerFactory)
    {
        _cosmosClient = cosmosClient;
        _logger = loggerFactory.CreateLogger<AddLink>();
    }

    [Function("AddLink")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "links")] HttpRequestData req)
    {
        _logger.LogInformation("AddLink (isolated) triggered.");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        _logger.LogInformation("Raw request body: " + requestBody);

        var jsonSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        var newLink = JsonConvert.DeserializeObject<LinkItem>(requestBody, jsonSettings);

        if (newLink == null || string.IsNullOrWhiteSpace(newLink.UserId))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid request.");
            return badResponse;
        }

        var container = _cosmosClient.GetContainer(_databaseId, _containerId);
        var result = await container.CreateItemAsync(newLink, new PartitionKey(newLink.UserId));

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonConvert.SerializeObject(result.Resource));

        return response;
    }
}
