namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public record class BuildingBranch(int Id, string Name, bool Interesting, ImmutableArray<BuildingLevel> Levels);