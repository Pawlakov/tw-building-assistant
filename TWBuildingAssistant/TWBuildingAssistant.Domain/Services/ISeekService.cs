﻿namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.StateModels;

public interface ISeekService
{
    Task<ImmutableArray<SeekerResult>> Seek(
        Settings settings,
        EffectSet predefinedEffect,
        ImmutableArray<BuildingLibraryEntry> buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<ProvinceState> minimalCondition,
        Func<long, Task> updateProgressMax,
        Func<long, Task> updateProgressValue);
}