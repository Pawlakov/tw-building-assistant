namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class TechnologyLevelRepository : IRepository<ITechnologyLevel>
    {
        private const string JsonFileName = @"Json\twa_data_technology_levels.json";

        private readonly JsonSource<ITechnologyLevel, TechnologyLevel> jsonSource;

        public TechnologyLevelRepository()
        {
            this.jsonSource = new JsonSource<ITechnologyLevel, TechnologyLevel>(JsonFileName);
        }

        public IList<ITechnologyLevel> DataSet => this.jsonSource.DataSet;
    }
}