using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using ZapLink.Api.Models;

public class DeleteLink
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger _logger;
    private readonly string _databaseId = "ZapLinkDB";
    private readonly string _containerId = "Links";

    public DeleteLink(CosmosClient cosmosClient, ILoggerFactory loggerFactory)
    {
        _cosmosClient = cosmosClient;
        _logger = loggerFactory.CreateLogger<DeleteLink>();
    }

    [Function("DeleteLink")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "links/{id}/{userId}")] HttpRequestData req,
        string id, string userId)
    {
        _logger.LogInformation($"DeleteLink triggered for id: {id}");

        var container = _cosmosClient.GetContainer(_databaseId, _containerId);
        await container.DeleteItemAsync<LinkItem>(id, new PartitionKey(userId));

        var response = req.CreateResponse(HttpStatusCode.NoContent);
        return response;
    }
}
