namespace TWBuildingAssistant.StaticApp.Shared.DTOs;

public class SettingOptionsDTO
{
    public NamedStringIdDTO[] Provinces { get; set; }

    public NamedStringIdDTO[] Weathers { get; set; }

    public NamedStringIdDTO[] Seasons { get; set; }

    public NamedStringIdDTO[] Religions { get; set; }

    public NamedStringIdDTO[] Factions { get; set; }

    public NamedIdDTO[] Difficulties { get; set; }

    public NamedIdDTO[] Taxes { get; set; }

    public NamedIdDTO[] PowerLevels { get; set; }
}
