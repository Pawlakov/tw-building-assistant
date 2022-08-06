namespace TWBuildingAssistant.Data.Tool.HostBuilders;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class AddConfigurationHostBuilderExtensions
{
    public static IHostBuilder AddConfiguration(this IHostBuilder host)
    {
        host.ConfigureAppConfiguration(c =>
        {
            c.AddJsonFile("appsettings.json");
            c.AddEnvironmentVariables();
        });

        return host;
    }
}