namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class ClimateRepository : IRepository<IClimate>
    {
        private const string JsonFileName = @"Json\twa_data_climates.json";

        private readonly JsonSource<IClimate, Climate> jsonSource;

        public ClimateRepository()
        {
            this.jsonSource = new JsonSource<IClimate, Climate>(JsonFileName);
        }

        public IList<IClimate> DataSet => this.jsonSource.DataSet;
    }
}