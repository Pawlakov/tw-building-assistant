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
using TWBuildingAssistant.Data.Sqlite;

public class SeekerFunction
{
    private readonly ILogger logger;

    public SeekerFunction(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<SeekerFunction>();
    }

    [Function("GetSettingOptions")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        var model = Interface.getSettingOptions();
        var dto = new SettingOptionsDTO
        {
            Provinces = model.Provinces.Select(x => new NamedStringIdDTO { StringId = x.StringId, Name = x.Name }).ToArray(),
            Weathers = model.Weathers.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Seasons = model.Seasons.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Religions = model.Religions.Select(x => new NamedStringIdDTO { StringId = x.StringId, Name = x.Name }).ToArray(),
            Factions = model.Factions.Select(x => new NamedStringIdDTO { StringId = x.StringId, Name = x.Name }).ToArray(),
            Difficulties = model.Difficulties.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            Taxes = model.Taxes.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
            PowerLevels = model.PowerLevels.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(dto);

        return response;
    }
}