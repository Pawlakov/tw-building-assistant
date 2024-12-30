namespace TWBuildingAssistant.StaticApp.Api;

using System;
using System.Linq;
using TWBuildingAssistant.StaticApp.Shared.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using TWBuildingAssistant.Domain;

public class SeekerFunction
{
    private readonly ILogger logger;

    public SeekerFunction(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<SeekerFunction>();
    }

    [Function("GetSettingOptions")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var model = Interface.getSettingOptions();
        var provinceModel = Interface.getProvinceOptions();
        var dto = new SettingOptionsDTO
        {
            Provinces = provinceModel.Provinces.Select(x => new NamedIdDTO { Id = x.Id, Name = x.Name }).ToArray(),
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