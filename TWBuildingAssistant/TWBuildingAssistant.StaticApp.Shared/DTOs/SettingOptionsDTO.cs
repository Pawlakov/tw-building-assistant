namespace TWBuildingAssistant.StaticApp.Shared.DTOs;

public class SettingOptionsDTO
{
    public NamedIdDTO[] Provinces { get; set; }

    public NamedIdDTO[] Weathers { get; set; }

    public NamedIdDTO[] Seasons { get; set; }

    public NamedIdDTO[] Religions { get; set; }

    public NamedStringIdDTO[] Factions { get; set; }

    public NamedIdDTO[] Difficulties { get; set; }

    public NamedIdDTO[] Taxes { get; set; }

    public NamedIdDTO[] PowerLevels { get; set; }
}
