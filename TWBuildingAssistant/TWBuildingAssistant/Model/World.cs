namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Data;
    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Combinations;
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Factions;
    using TWBuildingAssistant.Model.Map;
    using TWBuildingAssistant.Model.Religions;
    using TWBuildingAssistant.Model.Resources;
    using TWBuildingAssistant.Model.Weather;

    using Unity;

    public partial class World
    {
        private readonly IUnityContainer resolver;

        private readonly WeatherManager weatherManager;

        private readonly ClimateManager climateManager;

        private readonly ResourceManager resourcesManager;

        private readonly ReligionsManager religionsManager;

        private readonly ProvincesManager provincesManager;

        private readonly FactionsManager factionsManager;

        private World()
        {
            this.resolver = new UnityContainer();
            this.resolver.RegisterInstance<ISource>(new JsonData());

            this.resourcesManager = new ResourceManager(this.resolver);
            this.resolver.RegisterInstance<Parser<IResource>>(this.resourcesManager);

            this.religionsManager = new ReligionsManager(this.resolver);
            this.resolver.RegisterInstance<Parser<IReligion>>(this.religionsManager);
            this.resolver.RegisterInstance<IStateReligionTracker>(this.religionsManager);

            this.weatherManager = new WeatherManager(this.resolver);
            this.resolver.RegisterInstance<Parser<IWeather>>(this.weatherManager);
            this.resolver.RegisterInstance<IConsideredWeatherTracker>(this.weatherManager);

            this.climateManager = new ClimateManager(this.resolver);
            this.resolver.RegisterInstance<Parser<IClimate>>(this.climateManager);

            this.provincesManager = new ProvincesManager(this.resolver);
            this.factionsManager = new FactionsManager(this.resolver);
        }

        public SimulationKit AssembleSimulationKit(WorldSettings settings)
        {
            this.weatherManager.ChangeConsideredWeather(settings.ConsideredWeathers);
            //
            this.religionsManager.ChangeStateReligion(settings.StateReligionIndex);
            //
            this.provincesManager.ChangeFertilityDrop(settings.FertilityDrop);
            //
            this.factionsManager.ChangeFaction(settings.FactionIndex);
            this.factionsManager.Faction.ChangeDesiredTechnologyLevel(settings.DesiredTechnologyLevelIndex, settings.UseLegacyTechnologies);
            //
            var combination = new Combination(this.provincesManager.Find(settings.ProvinceIndex));
            var pool = this.factionsManager.Faction.Buildings;
            //
            return new SimulationKit(pool, combination);
        }

        public IEnumerable<KeyValuePair<int, string>> Weathers => this.weatherManager.AllWeathersNames;

        public IEnumerable<KeyValuePair<int, string>> Religions => this.religionsManager.AllReligionsNames;

        public IEnumerable<KeyValuePair<int, string>> Provinces => this.provincesManager.AllProvincesNames;

        public IEnumerable<KeyValuePair<int, string>> Factions => this.factionsManager.AllFactionsNames;

        public IProvincialEffect Environment => this.factionsManager.Effect.Aggregate(this.religionsManager.StateReligion.Effect);
    }

    public partial class World
    {
        private static World world;

        public static World GetWorld()
        {
            return world ?? (world = new World());
        }
    }
}