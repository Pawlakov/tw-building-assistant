namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;
using System.Linq;

public static class BuildingLevelOperations
{
    public static BuildingLevel Empty { get; } = new BuildingLevel(0, "Empty", default, Enumerable.Empty<Income>().ToImmutableArray(), Enumerable.Empty<Influence>().ToImmutableArray());
}