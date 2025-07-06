using System;
using System.ComponentModel.DataAnnotations;

namespace ZapLink.Models;

public class LinkItem
{
    public int Id { get; set; }
    public string? UserId { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    [Url]
    public string Url { get; set; } = "";

    public List<string> Tags { get; set; } = new();

}
