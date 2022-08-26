namespace TWBuildingAssistant.Data.HostBuilders;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class ConfigurationHostBuilderExtensions
{
    public static IHostBuilder AddConfiguration(this IHostBuilder host)
    {
        return host.ConfigureAppConfiguration(c =>
        {
            c.AddJsonFile("appsettings.json");
            c.AddEnvironmentVariables();
        });
    }
}