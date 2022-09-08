namespace TWBuildingAssistant.Server.Controllers;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Shared.DTOs;

[Route("api/[controller]")]
[ApiController]
public class SeekerController
    : ControllerBase
{
    private readonly ILogger<SeekerController> logger;

    public SeekerController(ILogger<SeekerController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("options")]
    public async Task<ActionResult<SettingOptionsDTO>> GetSettingOptions()
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

            var model = await Task.Run(() => Settings.getOptions(connString));
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

            return Ok(dto);
    }
}
