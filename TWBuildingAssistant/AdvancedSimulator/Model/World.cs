namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public class World
    {
        private readonly Model.Resources.ResourcesManager _resourcesManager;

        private readonly Model.Religions.ReligionsManager _religionsManager;

        private readonly Model.Map.ProvincesManager _provincesManager;

        private readonly Model.Factions.FactionsManager _factionsManager;

        public World()
        {
            using (var database = new Data.DataModel())
            {
                this._resourcesManager = new Model.Resources.ResourcesManager(new Model.Resources.ResourcesDataSource(database));
                this._religionsManager = new Model.Religions.ReligionsManager();
                this._provincesManager = new Model.Map.ProvincesManager(this._religionsManager, this._resourcesManager, this._religionsManager);
                this._factionsManager = new Model.Factions.FactionsManager(this._religionsManager, this._resourcesManager);
            }
        }

        public SimulationKit AssembleSimulationKit(WorldSettings settings)
        {
            this._religionsManager.ChangeStateReligion(settings.StateReligionIndex);
            //
            this._provincesManager.ChangeFertilityDrop(settings.FertilityDrop);
            this._provincesManager.ChangeProvince(settings.ProvinceIndex);
            this._provincesManager.ChangeWorstCaseWeather(settings.WorstCaseWeather);
            //
            this._factionsManager.ChangeFaction(settings.FactionIndex);
            this._factionsManager.Faction.ChangeDesiredTechnologyLevel(settings.DesiredTechnologyLevelIndex, settings.UseLegacyTechnologies);
            //
            Model.Combinations.Combination combination = new Model.Combinations.Combination(this._provincesManager.Province);
            Model.Buildings.BuildingLibrary pool = this._factionsManager.Faction.Buildings;
            //
            return new SimulationKit(this, pool, combination);
        }

        public IEnumerable<KeyValuePair<int, string>> Religions
        {
            get
            {
                return this._religionsManager.AllReligionsNames;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> Provinces
        {
            get
            {
                return this._provincesManager.AllProvincesNames;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> Factions
        {
            get
            {
                return this._factionsManager.AllFactionsNames;
            }
        }

        public Model.Effects.ProvincionalEffectsPackage Environment
        {
            get
            {
                return new Model.Effects.ProvincionalEffectsPackage(
                this._religionsManager.PublicOrder + this._provincesManager.PublicOrder + this._factionsManager.PublicOrder,
                this._religionsManager.Food + this._provincesManager.Food + this._factionsManager.Food,
                this._religionsManager.Sanitation + this._provincesManager.Sanitation + this._factionsManager.Sanitation,
                this._religionsManager.ReligiousOsmosis + this._provincesManager.ReligiousOsmosis +
                this._factionsManager.ReligiousOsmosis,
                this._religionsManager.ReligiousInfluence + this._provincesManager.ReligiousInfluence +
                this._factionsManager.ReligiousInfluence,
                this._religionsManager.ResearchRate + this._provincesManager.ResearchRate + this._factionsManager.ResearchRate,
                this._religionsManager.Growth + this._provincesManager.Growth + this._factionsManager.Growth,
                this._religionsManager.Fertility + this._provincesManager.Fertility + this._factionsManager.Fertility,
                this._religionsManager.Bonuses.Concat(this._provincesManager.Bonuses.Concat(this._factionsManager.Bonuses)));
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