namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;

    public class World
    {
        private readonly Model.Resources.ResourcesManager resourcesManager;

        private readonly Model.Religions.ReligionsManager religionsManager;

        private readonly Model.Map.ProvincesManager provincesManager;

        private readonly Model.Factions.FactionsManager factionsManager;

        public World()
        {
            using (var database = new Data.DataModel())
            {
                this.resourcesManager = new Model.Resources.ResourcesManager(new Resources.ResourcesDataSource(database));
                this.religionsManager = new Model.Religions.ReligionsManager(new Religions.ReligionsDataSource(database));
                this.provincesManager = new Model.Map.ProvincesManager(this.religionsManager, this.resourcesManager, this.religionsManager);
                this.factionsManager = new Model.Factions.FactionsManager(this.religionsManager, this.resourcesManager);
            }
        }

        public SimulationKit AssembleSimulationKit(WorldSettings settings)
        {
            this.religionsManager.ChangeStateReligion(settings.StateReligionIndex);
            //
            this.provincesManager.ChangeFertilityDrop(settings.FertilityDrop);
            this.provincesManager.ChangeProvince(settings.ProvinceIndex);
            this.provincesManager.ChangeWorstCaseWeather(settings.WorstCaseWeather);
            //
            this.factionsManager.ChangeFaction(settings.FactionIndex);
            this.factionsManager.Faction.ChangeDesiredTechnologyLevel(settings.DesiredTechnologyLevelIndex, settings.UseLegacyTechnologies);
            //
            Combinations.Combination combination = new Model.Combinations.Combination(this.provincesManager.Province);
            Buildings.BuildingLibrary pool = this.factionsManager.Faction.Buildings;
            //
            return new SimulationKit(this, pool, combination);
        }

        public IEnumerable<KeyValuePair<int, string>> Religions
        {
            get
            {
                return this.religionsManager.AllReligionsNames;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> Provinces
        {
            get
            {
                return this.provincesManager.AllProvincesNames;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> Factions
        {
            get
            {
                return this.factionsManager.AllFactionsNames;
            }
        }

        public Model.Effects.IProvincionalEffect Environment
        {
            get
            {
                return this.provincesManager.Effect.Aggregate(this.factionsManager.Effect.Aggregate(this.religionsManager.StateReligion.Effect));
            }
        }
    }

    public class WorldSettings
    {
        public int StateReligionIndex { get; set; }

        public int FertilityDrop { get; set; }

        public Model.ClimateAndWeather.Weather WorstCaseWeather { get; set; }

        public int FactionIndex { get; set; }

        public int DesiredTechnologyLevelIndex { get; set; }

        public bool UseLegacyTechnologies { get; set; }

        public int ProvinceIndex { get; set; }
    }
}