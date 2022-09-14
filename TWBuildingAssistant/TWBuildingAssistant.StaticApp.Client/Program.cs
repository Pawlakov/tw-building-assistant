using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TWBuildingAssistant.StaticApp.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    {
        var config = sp.GetService<IConfiguration>();

        var client = new HttpClient
        {
            BaseAddress = new Uri("https://twa-api.azurewebsites.net"),
        };

        client.DefaultRequestHeaders.Add("x-functions-key", config["TWA_APIKEY"]);
        return client;
    });

await builder.Build().RunAsync();