namespace TWBuildingAssistant.Data.Test.Utils
{
    using SimpleInjector;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    public class JsonRepositoryResolver
    {
        private readonly Container container;

        static JsonRepositoryResolver()
        {
            Instance = new JsonRepositoryResolver();
        }

        private JsonRepositoryResolver()
        {
            this.container = new Container();
            this.container.Register<IRepository<IBonus>, BonusRepository>();
            this.container.Register<IRepository<IBuildingBranch>, BuildingBranchRepository>();
            this.container.Register<IRepository<IBuildingBranchUse>, BuildingBranchUseRepository>();
            this.container.Register<IRepository<IBuildingLevel>, BuildingLevelRepository>();
            this.container.Register<IRepository<IBuildingLevelLock>, BuildingLevelLockRepository>();
            this.container.Register<IRepository<IClimate>, ClimateRepository>();
            this.container.Register<IRepository<IFaction>, FactionRepository>();
            this.container.Register<IRepository<IInfluence>, InfluenceRepository>();
            this.container.Register<IRepository<IProvince>, ProvinceRepository>();
            this.container.Register<IRepository<IProvincialEffect>, ProvincialEffectRepository>();
            this.container.Register<IRepository<IRegion>, RegionRepository>();
            this.container.Register<IRepository<IRegionalEffect>, RegionalEffectRepository>();
            this.container.Register<IRepository<IReligion>, ReligionRepository>();
            this.container.Register<IRepository<IResource>, ResourceRepository>();
            this.container.Register<IRepository<ITechnologyLevel>, TechnologyLevelRepository>();
            this.container.Register<IRepository<IWeather>, WeatherRepository>();
            this.container.Register<IRepository<IWeatherEffect>, WeatherEffectRepository>();
        }

        public static JsonRepositoryResolver Instance { get; private set; }

        public IRepository<T> Resolve<T>()
        {
            return this.container.GetInstance<IRepository<T>>();
        }
    }
}