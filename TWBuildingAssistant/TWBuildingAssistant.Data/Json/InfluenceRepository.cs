namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class InfluenceRepository : IRepository<IInfluence>
    {
        private const string JsonFileName = @"Json\twa_data_influences.json";

        private readonly JsonSource<IInfluence, Influence> jsonSource;

        public InfluenceRepository()
        {
            this.jsonSource = new JsonSource<IInfluence, Influence>(JsonFileName);
        }

        public IList<IInfluence> DataSet => this.jsonSource.DataSet;
    }
}