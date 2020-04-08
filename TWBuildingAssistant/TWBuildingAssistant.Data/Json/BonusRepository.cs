namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class BonusRepository : IRepository<IBonus>
    {
        private const string JsonFileName = @"Json\twa_data_bonuses.json";

        private readonly JsonSource<IBonus, Bonus> jsonSource;

        public BonusRepository()
        {
            this.jsonSource = new JsonSource<IBonus, Bonus>(JsonFileName);
        }

        public IList<IBonus> DataSet => this.jsonSource.DataSet;
    }
}