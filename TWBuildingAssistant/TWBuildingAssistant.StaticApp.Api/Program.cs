namespace TWBuildingAssistant.StaticApp.Api;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Data.HostBuilders;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(configBuilder => configBuilder.AddEnvironmentVariables())
            //.AddConfiguration()
            //.AddDbContextAzure()
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        host.Run();
    }
}