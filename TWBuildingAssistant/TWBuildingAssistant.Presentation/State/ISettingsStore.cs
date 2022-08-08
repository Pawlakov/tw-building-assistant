namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsStore
{
    FactionSettings CurrentFactionSettings { get; set; }

    ProvinceSettings CurrentProvinceSettings { get; set; }

    int ProvinceId { get; set; }
}
