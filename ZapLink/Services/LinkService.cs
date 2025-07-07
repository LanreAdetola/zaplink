using System.Net.Http;
using System.Net.Http.Json;
using ZapLink.Models;

namespace ZapLink.Services;

public class LinkService
{
    private readonly HttpClient _http;

    public LinkService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<LinkItem>> GetLinksByUser(string userId)
    {
        return await _http.GetFromJsonAsync<List<LinkItem>>($"http://localhost:7071/api/links/{userId}")
               ?? new List<LinkItem>();
    }

    public async Task AddLink(LinkItem link)
    {
        var response = await _http.PostAsJsonAsync("http://localhost:7071/api/links", link);
        response.EnsureSuccessStatusCode();
    }


    public async Task UpdateLink(LinkItem link)
{
    await _http.PutAsJsonAsync($"http://localhost:7071/api/links/{link.Id}", link);
}

public async Task DeleteLink(string id, string userId)
{
    await _http.DeleteAsync($"http://localhost:7071/api/links/{id}/{userId}");
}

}
