using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZapLink;
using ZapLink.Services;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Register base HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Load config via HTTP (not filesystem)
var env = builder.HostEnvironment.IsDevelopment() ? "Development" : "Production";
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var configUrl = $"appsettings.{env}.json";

try
{
    using var stream = await httpClient.GetStreamAsync(configUrl);
    builder.Configuration.AddJsonStream(stream);
}
catch (HttpRequestException ex)
{
    Console.Error.WriteLine($"⚠️ Failed to load {configUrl}: {ex.Message}");
    // You may choose to fail silently or fallback here
}

// Register services
builder.Services.AddScoped<LinkService>();

await builder.Build().RunAsync();
