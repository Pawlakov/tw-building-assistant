namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Data;
    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Factions;
    using TWBuildingAssistant.Model.Map;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;
    using TWBuildingAssistant.Model.Weather;

    public class World
    {
        private readonly WeatherManager weatherManager;

        private readonly ClimateManager climateManager;

        private readonly ResourcesManager resourcesManager;

        private readonly ReligionsManager religionsManager;

        private readonly ProvincesManager provincesManager;

        private readonly FactionsManager factionsManager;

        public World()
        {
            this.resourcesManager = new ResourcesManager(JsonData.GetData());
            this.religionsManager = new ReligionsManager(JsonData.GetData());
            this.weatherManager = new WeatherManager(JsonData.GetData());
            this.climateManager = new ClimateManager(JsonData.GetData(), this.weatherManager, this.weatherManager, this.religionsManager);
            this.provincesManager = new ProvincesManager(this.religionsManager, this.resourcesManager, this.climateManager);
            this.factionsManager = new FactionsManager(this.religionsManager, this.resourcesManager);
        }

        public SimulationKit AssembleSimulationKit(WorldSettings settings)
        {
            this.weatherManager.ChangeConsideredWeather(settings.ConsideredWeathers);
            //
            this.religionsManager.ChangeStateReligion(settings.StateReligionIndex);
            //
            this.provincesManager.ChangeFertilityDrop(settings.FertilityDrop);
            this.provincesManager.ChangeProvince(settings.ProvinceIndex);
            //
            this.factionsManager.ChangeFaction(settings.FactionIndex);
            this.factionsManager.Faction.ChangeDesiredTechnologyLevel(settings.DesiredTechnologyLevelIndex, settings.UseLegacyTechnologies);
            //
            Combinations.Combination combination = new Model.Combinations.Combination(this.provincesManager.Province);
            Buildings.BuildingLibrary pool = this.factionsManager.Faction.Buildings;
            //
            return new SimulationKit(this, pool, combination);
        }

        public IEnumerable<KeyValuePair<int, string>> Weathers
        {
            get
            {
                return this.weatherManager.AllWeathersNames;
            }
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

        public IProvincialEffect Environment
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

        public IEnumerable<int> ConsideredWeathers { get; set; }

        public int FactionIndex { get; set; }

        public int DesiredTechnologyLevelIndex { get; set; }

        public bool UseLegacyTechnologies { get; set; }

        public int ProvinceIndex { get; set; }
    }
}