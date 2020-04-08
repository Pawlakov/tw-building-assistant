namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class ProvinceRepository : IRepository<IProvince>
    {
        private const string JsonFileName = @"Json\twa_data_provinces.json";

        private readonly JsonSource<IProvince, Province> jsonSource;

        public ProvinceRepository()
        {
            this.jsonSource = new JsonSource<IProvince, Province>(JsonFileName);
        }

        public IList<IProvince> DataSet => this.jsonSource.DataSet;
    }
}