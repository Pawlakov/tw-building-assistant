namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public readonly record struct BuildingBranch(int Id, string Name, ImmutableArray<BuildingLevel> Levels);