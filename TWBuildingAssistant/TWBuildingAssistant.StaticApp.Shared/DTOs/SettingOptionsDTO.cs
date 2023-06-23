namespace TWBuildingAssistant.StaticApp.Shared.DTOs;

public class SettingOptionsDTO
{
    public NamedStringIdDTO[] Provinces { get; set; }

    public NamedIdDTO[] Weathers { get; set; }

    public NamedIdDTO[] Seasons { get; set; }

    public NamedStringIdDTO[] Religions { get; set; }

    public NamedStringIdDTO[] Factions { get; set; }

    public NamedIdDTO[] Difficulties { get; set; }

    public NamedIdDTO[] Taxes { get; set; }

    public NamedIdDTO[] PowerLevels { get; set; }
}
