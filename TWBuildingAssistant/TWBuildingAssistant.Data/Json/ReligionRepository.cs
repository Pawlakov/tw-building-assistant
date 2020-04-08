namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class ReligionRepository : IRepository<IReligion>
    {
        private const string JsonFileName = @"Json\twa_data_religions.json";

        private readonly JsonSource<IReligion, Religion> jsonSource;

        public ReligionRepository()
        {
            this.jsonSource = new JsonSource<IReligion, Religion>(JsonFileName);
        }

        public IList<IReligion> DataSet => this.jsonSource.DataSet;
    }
}