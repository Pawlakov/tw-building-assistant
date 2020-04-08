namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class BuildingLevelRepository : IRepository<IBuildingLevel>
    {
        private const string JsonFileName = @"Json\twa_data_building_levels.json";

        private readonly JsonSource<IBuildingLevel, BuildingLevel> jsonSource;

        public BuildingLevelRepository()
        {
            this.jsonSource = new JsonSource<IBuildingLevel, BuildingLevel>(JsonFileName);
        }

        public IList<IBuildingLevel> DataSet => this.jsonSource.DataSet;
    }
}