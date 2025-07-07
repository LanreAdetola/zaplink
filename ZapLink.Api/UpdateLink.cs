using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using ZapLink.Api.Models;
using Newtonsoft.Json;

public class UpdateLink
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger _logger;
    private readonly string _databaseId = "ZapLinkDB";
    private readonly string _containerId = "Links";

    public UpdateLink(CosmosClient cosmosClient, ILoggerFactory loggerFactory)
    {
        _cosmosClient = cosmosClient;
        _logger = loggerFactory.CreateLogger<UpdateLink>();
    }

    [Function("UpdateLink")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "links/{id}")] HttpRequestData req,
        string id)
    {
        _logger.LogInformation($"UpdateLink triggered for id: {id}");

        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var updatedLink = JsonConvert.DeserializeObject<LinkItem>(body);

        if (updatedLink == null || string.IsNullOrWhiteSpace(updatedLink.UserId))
        {
            var badRes = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRes.WriteStringAsync("Invalid data.");
            return badRes;
        }

        var container = _cosmosClient.GetContainer(_databaseId, _containerId);
        var response = await container.ReplaceItemAsync(updatedLink, id, new PartitionKey(updatedLink.UserId));

        var result = req.CreateResponse(HttpStatusCode.OK);
        await result.WriteStringAsync(JsonConvert.SerializeObject(response.Resource));
        return result;
    }
}
