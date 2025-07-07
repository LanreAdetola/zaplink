using System.Net.Http;
using System.Net.Http.Json;
using ZapLink.Models;

namespace ZapLink.Services;

public class LinkService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public LinkService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config["ApiUrl"] ?? "http://localhost:7071";
    }

    public async Task<List<LinkItem>> GetLinksByUser(string userId)
    {
        return await _http.GetFromJsonAsync<List<LinkItem>>($"{_baseUrl}/api/links/{userId}")
               ?? new List<LinkItem>();
    }

    public async Task AddLink(LinkItem link)
    {
        var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/links", link);
        response.EnsureSuccessStatusCode();
    }


    public async Task UpdateLink(LinkItem link)
{
    await _http.PutAsJsonAsync($"{_baseUrl}/api/links/{link.Id}", link);
}

public async Task DeleteLink(string id, string userId)
{
    await _http.DeleteAsync($"{_baseUrl}/api/links/{id}/{userId}");
}

}
