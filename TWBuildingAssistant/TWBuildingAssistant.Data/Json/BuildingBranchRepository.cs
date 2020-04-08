namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class BuildingBranchRepository : IRepository<IBuildingBranch>
    {
        private const string JsonFileName = @"Json\twa_data_building_branches.json";

        private readonly JsonSource<IBuildingBranch, BuildingBranch> jsonSource;

        public BuildingBranchRepository()
        {
            this.jsonSource = new JsonSource<IBuildingBranch, BuildingBranch>(JsonFileName);
        }

        public IList<IBuildingBranch> DataSet => this.jsonSource.DataSet;
    }
}