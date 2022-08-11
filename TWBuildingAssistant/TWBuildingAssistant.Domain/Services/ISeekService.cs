namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public interface ISeekService
{
    void Seek(
        Settings settings,
        EffectSet predefinedEffect,
        ImmutableArray<BuildingLibraryEntry> buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<ProvinceState> minimalCondition,
        Action<long> updateProgressMax,
        Action<long> updateProgressValue);
}