namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.StateModels;

public interface ISeekService
{
    ImmutableArray<SeekerResult> Seek(
        Data.FSharp.Models.Settings settings,
        Data.FSharp.Models.EffectSet predefinedEffect,
        Data.FSharp.Models.BuildingLibraryEntry[] buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<ProvinceState> minimalCondition,
        Func<long, Task> updateProgressMax,
        Func<long, Task> updateProgressValue);
}