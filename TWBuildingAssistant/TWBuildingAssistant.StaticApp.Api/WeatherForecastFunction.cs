namespace TWBuildingAssistant.StaticApp.Api;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using TWBuildingAssistant.StaticApp.Shared;

public class WeatherForecastFunction
{
    private readonly ILogger logger;

    public WeatherForecastFunction(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<WeatherForecastFunction>();
    }

    [Function("WeatherForecast")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "WeatherForecast")] HttpRequestData req)
    {
        var randomNumber = new Random();
        var temp = 0;

        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = temp = randomNumber.Next(-20, 55),
            Summary = GetSummary(temp)
        }).ToArray();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(result);

        return response;
    }

    private string GetSummary(int temp)
    {
        var summary = "Mild";

        if (temp >= 32)
        {
            summary = "Hot";
        }
        else if (temp <= 16 && temp > 0)
        {
            summary = "Cold";
        }
        else if (temp <= 0)
        {
            summary = "Freezing!";
        }

        return summary;
    }
}