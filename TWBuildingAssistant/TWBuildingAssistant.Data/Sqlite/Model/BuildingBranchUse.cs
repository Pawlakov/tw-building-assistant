namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class BuildingBranchUse : IBuildingBranchUse
    {
        public BuildingBranchUse()
        {
        }

        public BuildingBranchUse(IBuildingBranchUse source)
        {
            this.FactionId = source.FactionId;
            this.BuildingBranchId = source.BuildingBranchId;
        }

        public int FactionId { get; set; }

        public int BuildingBranchId { get; set; }
    }
}