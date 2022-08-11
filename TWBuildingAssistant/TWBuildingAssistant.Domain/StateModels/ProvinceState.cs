namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;

public readonly record struct ProvinceState(ImmutableArray<RegionState> Regions, int Food, int PublicOrder, int ReligiousOsmosis, int ResearchRate, int Growth, double Wealth);