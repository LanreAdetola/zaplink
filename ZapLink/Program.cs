using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZapLink;
using ZapLink.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Load config from wwwroot/appsettings.*.json
var config = await builder.Configuration.AddJsonStream(
    await builder.HostEnvironment.IsDevelopment()
        ? File.OpenReadAsync("appsettings.Development.json")
        : File.OpenReadAsync("appsettings.Production.json")
);

builder.Services.AddScoped<LinkService>();

await builder.Build().RunAsync();
