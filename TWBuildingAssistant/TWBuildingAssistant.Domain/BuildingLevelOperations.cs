namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public static class BuildingLevelOperations
{
    public static BuildingLevel Empty { get; } = new BuildingLevel(0, "Empty", default, ImmutableArray.Create<Income>(), ImmutableArray.Create<Influence>());
}