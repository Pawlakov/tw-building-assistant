namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class FactionRepository : IRepository<IFaction>
    {
        private const string JsonFileName = @"Json\twa_data_factions.json";

        private readonly JsonSource<IFaction, Faction> jsonSource;

        public FactionRepository()
        {
            this.jsonSource = new JsonSource<IFaction, Faction>(JsonFileName);
        }

        public IList<IFaction> DataSet => this.jsonSource.DataSet;
    }
}