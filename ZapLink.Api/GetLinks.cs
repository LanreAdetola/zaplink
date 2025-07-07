using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using ZapLink.Api.Models;
using System.Text.Json;
using Newtonsoft.Json;

public class GetLinks
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger _logger;
    private readonly string _databaseId = "ZapLinkDB";
    private readonly string _containerId = "Links";

    public GetLinks(CosmosClient cosmosClient, ILoggerFactory loggerFactory)
    {
        _cosmosClient = cosmosClient;
        _logger = loggerFactory.CreateLogger<GetLinks>();
    }

    [Function("GetLinks")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "links/{userId}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation($"GetLinks triggered for userId: {userId}");

        var container = _cosmosClient.GetContainer(_databaseId, _containerId);

        var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId")
            .WithParameter("@userId", userId);

        var iterator = container.GetItemQueryIterator<LinkItem>(query, requestOptions: new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(userId)
        });

        var results = new List<LinkItem>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        var responseData = req.CreateResponse(HttpStatusCode.OK);
        var json = JsonConvert.SerializeObject(results);
        await responseData.WriteStringAsync(json);


        return responseData;
    }
}
