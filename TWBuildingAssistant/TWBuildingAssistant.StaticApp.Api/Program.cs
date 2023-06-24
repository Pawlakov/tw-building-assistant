namespace TWBuildingAssistant.StaticApp.Api;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(configBuilder => configBuilder.AddEnvironmentVariables())
            .ConfigureFunctionsWorkerDefaults(builder =>
            {
                builder
                    .AddApplicationInsights()
                    .AddApplicationInsightsLogger();
            })
            .Build();

        host.Run();
    }
}