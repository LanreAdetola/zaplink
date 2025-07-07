using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ZapLink.Api.Models;

public class LinkItem
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("userId")]
    public string UserId { get; set; } = "";

    [Required]
    [JsonProperty("title")]
    public string Title { get; set; } = "";

    [Required]
    [JsonProperty("url")]
    public string Url { get; set; } = "";

    [JsonProperty("tags")]
    public List<string> Tags { get; set; } = new();
}
