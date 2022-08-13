namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public readonly record struct Province(int Id, string Name, ImmutableArray<Region> Regions);