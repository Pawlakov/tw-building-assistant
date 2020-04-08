namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class RegionRepository : IRepository<IRegion>
    {
        private const string JsonFileName = @"Json\twa_data_regions.json";

        private readonly JsonSource<IRegion, Region> jsonSource;

        public RegionRepository()
        {
            this.jsonSource = new JsonSource<IRegion, Region>(JsonFileName);
        }

        public IList<IRegion> DataSet => this.jsonSource.DataSet;
    }
}