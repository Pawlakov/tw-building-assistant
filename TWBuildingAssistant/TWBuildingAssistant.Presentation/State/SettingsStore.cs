﻿namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;

public class SettingsStore
    : ISettingsStore
{
    public Data.FSharp.Models.EffectSet Effect { get; set; }

    public ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry> BuildingLibrary { get; set; }
}