using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZapLink.Models;

public class LinkItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = "";

    [Required]
    public string Title { get; set; } = "";

    [Required]
    [Url]
    public string Url { get; set; } = "";

    public List<string> Tags { get; set; } = new();

    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
