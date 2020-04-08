namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class ProvincialEffectRepository : IRepository<IProvincialEffect>
    {
        private const string JsonFileName = @"Json\twa_data_provincial_effects.json";

        private readonly JsonSource<IProvincialEffect, ProvincialEffect> jsonSource;

        public ProvincialEffectRepository()
        {
            this.jsonSource = new JsonSource<IProvincialEffect, ProvincialEffect>(JsonFileName);
        }

        public IList<IProvincialEffect> DataSet => this.jsonSource.DataSet;
    }
}