namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class BuildingBranchUseRepository : IRepository<IBuildingBranchUse>
    {
        private const string JsonFileName = @"Json\twa_data_building_branch_uses.json";

        private readonly JsonSource<IBuildingBranchUse, BuildingBranchUse> jsonSource;

        public BuildingBranchUseRepository()
        {
            this.jsonSource = new JsonSource<IBuildingBranchUse, BuildingBranchUse>(JsonFileName);
        }

        public IList<IBuildingBranchUse> DataSet => this.jsonSource.DataSet;
    }
}