namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceService
{
    ProvinceState GetState(
        IEnumerable<IEnumerable<BuildingLevel>> buildings,
        Data.FSharp.Models.Settings settings,
        Data.FSharp.Models.EffectSet predefinedEffect);
}