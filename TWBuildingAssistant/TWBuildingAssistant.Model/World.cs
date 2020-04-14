namespace TWBuildingAssistant.Model
{
    //using SimpleInjector;
    //using System.Collections.Generic;
    //using System.Linq;
    //using TWBuildingAssistant.Data;
    //using TWBuildingAssistant.Data.Json;
    //using TWBuildingAssistant.Data.Model;
    //using TWBuildingAssistant.Model.Combinations;

    //public partial class World
    //{
    //    private readonly Container resolver;

    //    private World()
    //    {
    //        this.resolver = new Container();
    //        this.resolver.RegisterInstance<IRepository<IBonus>>(new BonusRepository());
    //        this.resolver.RegisterInstance<IRepository<IBuildingBranch>>(new BuildingBranchRepository());
    //        this.resolver.RegisterInstance<IRepository<IBuildingBranchUse>>(new BuildingBranchUseRepository());
    //        this.resolver.RegisterInstance<IRepository<IBuildingLevel>>(new BuildingLevelRepository());
    //        this.resolver.RegisterInstance<IRepository<IBuildingLevelLock>>(new BuildingLevelLockRepository());
    //        this.resolver.RegisterInstance<IRepository<IClimate>>(new ClimateRepository());
    //        this.resolver.RegisterInstance<IRepository<IFaction>>(new FactionRepository());
    //        this.resolver.RegisterInstance<IRepository<IInfluence>>(new InfluenceRepository());
    //        this.resolver.RegisterInstance<IRepository<IProvince>>(new ProvinceRepository());
    //        this.resolver.RegisterInstance<IRepository<IProvincialEffect>>(new ProvincialEffectRepository());
    //        this.resolver.RegisterInstance<IRepository<IRegion>>(new RegionRepository());
    //        this.resolver.RegisterInstance<IRepository<IRegionalEffect>>(new RegionalEffectRepository());
    //        this.resolver.RegisterInstance<IRepository<IReligion>>(new ReligionRepository());
    //        this.resolver.RegisterInstance<IRepository<IResource>>(new ResourceRepository());
    //        this.resolver.RegisterInstance<IRepository<ITechnologyLevel>>(new TechnologyLevelRepository());
    //        this.resolver.RegisterInstance<IRepository<IWeather>>(new WeatherRepository());
    //        this.resolver.RegisterInstance<IRepository<IWeatherEffect>>(new WeatherEffectRepository());

    //        Weathers = resolver.GetInstance<IRepository<IWeather>>().DataSet.OrderBy(x => x.Id).Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
    //        Religions = resolver.GetInstance<IRepository<IReligion>>().DataSet.OrderBy(x => x.Id).Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
    //        Provinces = resolver.GetInstance<IRepository<IProvince>>().DataSet.OrderBy(x => x.Id).Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
    //        Factions = resolver.GetInstance<IRepository<IFaction>>().DataSet.OrderBy(x => x.Id).Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
    //    }

    //    public SimulationKit AssembleSimulationKit(WorldSettings settings)
    //    {
    //        this.weatherManager.ChangeConsideredWeather(settings.ConsideredWeathers);
    //        //
    //        this.religionsManager.ChangeStateReligion(settings.StateReligionIndex);
    //        //
    //        this.provincesManager.ChangeFertilityDrop(settings.FertilityDrop);
    //        //
    //        this.factionsManager.ChangeFaction(settings.FactionIndex);
    //        this.factionsManager.Faction.ChangeDesiredTechnologyLevel(settings.DesiredTechnologyLevelIndex, settings.UseLegacyTechnologies);
    //        //
    //        var combination = new Combination(this.provincesManager.Find(settings.ProvinceIndex));
    //        var pool = this.factionsManager.Faction.Buildings;
    //        //
    //        return new SimulationKit(pool, combination);
    //    }

    //    public IEnumerable<KeyValuePair<int, string>> Weathers { get; }

    //    public IEnumerable<KeyValuePair<int, string>> Religions { get; }

    //    public IEnumerable<KeyValuePair<int, string>> Provinces { get; }

    //    public IEnumerable<KeyValuePair<int, string>> Factions { get; }

    //    public IProvincialEffect Environment => this.factionsManager.Effect.Aggregate(this.religionsManager.StateReligion.Effect);
    //}

    //public partial class World
    //{
    //    private static World world;

    //    public static World GetWorld()
    //    {
    //        return world ?? (world = new World());
    //    }
    //}
}