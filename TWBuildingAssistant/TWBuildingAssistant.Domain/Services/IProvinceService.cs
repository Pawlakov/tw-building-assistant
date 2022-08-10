namespace TWBuildingAssistant.Domain.Services;

using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceService
{
    ProvinceState GetState(
        Province province,
        in Settings settings,
        Faction faction,
        in Climate climate,
        in Religion religion);
}