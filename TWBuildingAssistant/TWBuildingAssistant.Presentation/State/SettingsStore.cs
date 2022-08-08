namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain.StateModels;

public class SettingsStore
    : ISettingsStore
{
    public FactionSettings CurrentFactionSettings { get; set; }

    public ProvinceSettings CurrentProvinceSettings { get; set; }

    public int ProvinceId { get; set; }
}