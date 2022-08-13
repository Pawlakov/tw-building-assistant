namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public static class BuildingBranchOperations
{
    public static BuildingBranch Empty { get; } = new BuildingBranch(0, "Empty", true, ImmutableArray.Create(BuildingLevelOperations.Empty));
}