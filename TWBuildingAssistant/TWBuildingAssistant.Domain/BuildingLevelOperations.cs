namespace TWBuildingAssistant.Domain;

using System.Linq;

public static class BuildingLevelOperations
{
    public static BuildingLevel Empty { get; } = new BuildingLevel(0, "Empty", Data.FSharp.Models.emptyEffect, Enumerable.Empty<Data.FSharp.Models.Income>(), Enumerable.Empty<Data.FSharp.Models.Influence>());
}