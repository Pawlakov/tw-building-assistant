namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class RegionalEffectRepository : IRepository<IRegionalEffect>
    {
        private const string JsonFileName = @"Json\twa_data_regional_effects.json";

        private readonly JsonSource<IRegionalEffect, RegionalEffect> jsonSource;

        public RegionalEffectRepository()
        {
            this.jsonSource = new JsonSource<IRegionalEffect, RegionalEffect>(JsonFileName);
        }

        public IList<IRegionalEffect> DataSet => this.jsonSource.DataSet;
    }
}