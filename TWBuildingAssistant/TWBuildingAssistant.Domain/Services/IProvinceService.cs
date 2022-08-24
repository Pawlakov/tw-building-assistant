namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceService
{
    ProvinceState GetState(
        IEnumerable<IEnumerable<Data.FSharp.Models.BuildingLevel>> buildings,
        Data.FSharp.Models.Settings settings,
        Data.FSharp.Models.EffectSet predefinedEffect);
}