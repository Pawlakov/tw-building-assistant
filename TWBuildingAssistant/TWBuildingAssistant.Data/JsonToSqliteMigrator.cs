namespace TWBuildingAssistant.Data
{
    using System.Linq;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;
    using TWBuildingAssistant.Data.Sqlite;
    using TWBuildingAssistant.Data.Sqlite.Model;

    public class JsonToSqliteMigrator
    {
        public void Run()
        {
            var context = new DatabaseContext();

            context.Bonuses.RemoveRange(context.Bonuses);
            context.BuildingBranches.RemoveRange(context.BuildingBranches);
            context.BuildingBranchUses.RemoveRange(context.BuildingBranchUses);
            context.BuildingLevels.RemoveRange(context.BuildingLevels);
            context.BuildingLevelLocks.RemoveRange(context.BuildingLevelLocks);
            context.Climates.RemoveRange(context.Climates);
            context.Factions.RemoveRange(context.Factions);
            context.Influences.RemoveRange(context.Influences);
            context.Provinces.RemoveRange(context.Provinces);
            context.ProvincialEffects.RemoveRange(context.ProvincialEffects);
            context.Regions.RemoveRange(context.Regions);
            context.RegionalEffects.RemoveRange(context.RegionalEffects);
            context.Religions.RemoveRange(context.Religions);
            context.Resources.RemoveRange(context.Resources);
            context.TechnologyLevels.RemoveRange(context.TechnologyLevels);
            context.Weathers.RemoveRange(context.Weathers);
            context.WeatherEffects.RemoveRange(context.WeatherEffects);

            context.Bonuses.AddRange(new JsonRepository<IBonus>().DataSet.Select(x => new Bonus(x)));
            context.BuildingBranches.AddRange(new JsonRepository<IBuildingBranch>().DataSet.Select(x => new BuildingBranch(x)));
            context.BuildingBranchUses.AddRange(new JsonRepository<IBuildingBranchUse>().DataSet.Select(x => new BuildingBranchUse(x)));
            context.BuildingLevels.AddRange(new JsonRepository<IBuildingLevel>().DataSet.Select(x => new BuildingLevel(x)));
            context.BuildingLevelLocks.AddRange(new JsonRepository<IBuildingLevelLock>().DataSet.Select(x => new BuildingLevelLock(x)));
            context.Climates.AddRange(new JsonRepository<IClimate>().DataSet.Select(x => new Climate(x)));
            context.Factions.AddRange(new JsonRepository<IFaction>().DataSet.Select(x => new Faction(x)));
            context.Influences.AddRange(new JsonRepository<IInfluence>().DataSet.Select(x => new Influence(x)));
            context.Provinces.AddRange(new JsonRepository<IProvince>().DataSet.Select(x => new Province(x)));
            context.ProvincialEffects.AddRange(new JsonRepository<IProvincialEffect>().DataSet.Select(x => new ProvincialEffect(x)));
            context.Regions.AddRange(new JsonRepository<IRegion>().DataSet.Select(x => new Region(x)));
            context.RegionalEffects.AddRange(new JsonRepository<IRegionalEffect>().DataSet.Select(x => new RegionalEffect(x)));
            context.Religions.AddRange(new JsonRepository<IReligion>().DataSet.Select(x => new Religion(x)));
            context.Resources.AddRange(new JsonRepository<IResource>().DataSet.Select(x => new Resource(x)));
            context.TechnologyLevels.AddRange(new JsonRepository<ITechnologyLevel>().DataSet.Select(x => new TechnologyLevel(x)));
            context.Weathers.AddRange(new JsonRepository<IWeather>().DataSet.Select(x => new Weather(x)));
            context.WeatherEffects.AddRange(new JsonRepository<IWeatherEffect>().DataSet.Select(x => new WeatherEffect(x)));

            context.SaveChanges();
        }
    }
}