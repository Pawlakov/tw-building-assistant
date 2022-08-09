namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TWBuildingAssistant.Data.Sqlite;

public class SettingsService
    : ISettingsService
{
    private readonly DatabaseContextFactory contextFactory;

    public SettingsService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<IEnumerable<NamedId>> GetFactionOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Factions
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetProvinceOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Provinces
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetReligionOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Religions
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetSeasonOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Seasons
                .OrderBy(x => x.Order)
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetWeatherOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Weathers
                .OrderBy(x => x.Order)
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }
}