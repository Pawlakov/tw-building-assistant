namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class BuildingBranchUse
{
    [ForeignKey(nameof(Faction))]
    public int FactionId { get; set; }

    [ForeignKey(nameof(BuildingBranch))]
    public int BuildingBranchId { get; set; }

    public Faction Faction { get; set; }

    public BuildingBranch BuildingBranch { get; set; }
}