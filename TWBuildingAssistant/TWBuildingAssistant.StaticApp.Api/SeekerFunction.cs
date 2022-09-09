namespace TWBuildingAssistant.StaticApp.Api;

using System;
using System.Linq;
using TWBuildingAssistant.StaticApp.Shared.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using TWBuildingAssistant.Domain;

public class SeekerFunction
{
    private readonly ILogger logger;

    public SeekerFunction(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<SeekerFunction>();
    }

    [Function("GetSettingOptions")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
    {
        var connString = (string)null;
        try
        {
            var client = new SecretClient(new Uri("https://twa-keys.vault.azure.net/"), new DefaultAzureCredential());
            var secret = client.GetSecret("twa-connstring");
            connString = secret.Value.Value;
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "Couldn't retrieve connection string from the vault.");
        }

        var model = Settings.getOptions(connString);
        var dto = new SettingOptionsDTO
        {
            Provinces = model.Provinces.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Weathers = model.Weathers.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Seasons = model.Seasons.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Religions = model.Religions.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Factions = model.Factions.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Difficulties = model.Difficulties.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Taxes = model.Taxes.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            PowerLevels = model.PowerLevels.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(dto);

        return response;
    }
}