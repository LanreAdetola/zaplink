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

        // Only allow the owner's userId
        const string ownerUserId = "153dc891d9a7446f84f682fdb33ca7d6";
        if (userId != ownerUserId)
        {
            var forbidden = req.CreateResponse(HttpStatusCode.Forbidden);
            await forbidden.WriteStringAsync("Not authorized.");
            return forbidden;
        }

        var container = _cosmosClient.GetContainer(_databaseId, _containerId);
        await container.DeleteItemAsync<LinkItem>(id, new PartitionKey(userId));

        var response = req.CreateResponse(HttpStatusCode.NoContent);
        return response;
    }
}
