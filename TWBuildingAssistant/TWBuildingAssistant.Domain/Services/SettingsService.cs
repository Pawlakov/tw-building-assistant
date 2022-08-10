namespace TWBuildingAssistant.Domain.Services;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

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

    public async Task<(Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences)> GetStateFromSettings(Settings settings)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var factionEffects = await this.GetFactionwideEffects(context, settings);
            var technologyEffects = await this.GetTechnologyEffect(context, settings);
            var religionEffects = await this.GetReligionEffect(context, settings);
            var provinceEffects = await this.GetProvinceEffect(context, settings);
            var climateEffects = await this.GetClimateEffect(context, settings);
            var fertilityDropEffect = EffectOperations.Create(fertility: settings.FertilityDrop);
            var corruptionIncome = IncomeOperations.Create(-settings.CorruptionRate, null, BonusType.Percentage);

            var effects = new[]
            {
                factionEffects.Effect,
                technologyEffects.Effect,
                religionEffects.Effect,
                provinceEffects.Effect,
                climateEffects.Effect,
                fertilityDropEffect,
            };

            var incomes = factionEffects.Incomes
                .Concat(technologyEffects.Incomes)
                .Concat(religionEffects.Incomes)
                .Concat(provinceEffects.Incomes)
                .Concat(climateEffects.Incomes)
                .Append(corruptionIncome);

            var influences = factionEffects.Influences
                .Concat(technologyEffects.Influences)
                .Concat(religionEffects.Influences)
                .Concat(provinceEffects.Influences)
                .Concat(climateEffects.Influences);

            return (EffectOperations.Collect(effects), incomes.ToImmutableArray(), influences.ToImmutableArray());
        }
    }

    private async Task<Triad> GetFactionwideEffects(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Factions
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => x.Id == settings.FactionId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private async Task<Triad> GetTechnologyEffect(DatabaseContext context, Settings settings)
    {
        var universalEffectEntities = await context.TechnologyLevels
            .AsNoTracking()
            .Include(x => x.UniversalEffect.Bonuses)
            .Include(x => x.UniversalEffect.Influences)
            .Where(x => x.FactionId == settings.FactionId)
            .Where(x => x.Order <= settings.TechnologyTier)
            .Select(x => x.UniversalEffect)
            .ToListAsync();

        var effects = universalEffectEntities.Select(x => this.MakeEffect(x));

        if (settings.UseAntilegacyTechnologies)
        {
            var antilegacyEffectEntities = await context.TechnologyLevels
                .AsNoTracking()
                .Include(x => x.AntilegacyEffect.Bonuses)
                .Include(x => x.AntilegacyEffect.Influences)
                .Where(x => x.FactionId == settings.FactionId)
                .Where(x => x.Order <= settings.TechnologyTier)
                .Select(x => x.AntilegacyEffect)
                .Include(x => x.Bonuses)
                .Include(x => x.Influences)
                .ToListAsync();

            effects = effects.Concat(antilegacyEffectEntities.Select(x => this.MakeEffect(x)));
        }

        return new Triad(EffectOperations.Collect(effects.Select(x => x.Effect)), effects.SelectMany(x => x.Incomes).ToImmutableArray(), effects.SelectMany(x => x.Influences).ToImmutableArray());
    }

    private async Task<Triad> GetReligionEffect(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Religions
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => x.Id == settings.ReligionId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private async Task<Triad> GetProvinceEffect(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Provinces
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => x.Id == settings.ProvinceId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private async Task<Triad> GetClimateEffect(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Provinces
            .AsNoTracking()
            .Include(x => x.Climate.Effects)
            .ThenInclude(x => x.Effect.Influences)
            .Include(x => x.Climate.Effects)
            .ThenInclude(x => x.Effect.Influences)
            .Where(x => x.Id == settings.ProvinceId)
            .SelectMany(x => x.Climate.Effects)
            .Where(x => x.WeatherId == settings.WeatherId && x.SeasonId == settings.SeasonId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private Triad MakeEffect(Data.Sqlite.Entities.Effect? effectEntity)
    {
        Effect effect = default;
        IEnumerable<Income> incomes = Enumerable.Empty<Income>();
        IEnumerable<Influence> influences = Enumerable.Empty<Influence>();
        if (effectEntity is not null)
        {
            effect = EffectOperations.Create(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, effectEntity.RegionalSanitation);
            if (effectEntity.Bonuses is not null)
            {
                incomes = effectEntity.Bonuses.Select(x => IncomeOperations.Create(x.Value, x.Category, x.Type));
            }

            if (effectEntity.Influences is not null)
            {
                influences = effectEntity.Influences.Select(x => InfluenceOperations.Create(x.ReligionId, x.Value));
            }
        }

        return new Triad(effect, incomes.ToImmutableArray(), influences.ToImmutableArray());
    }

    private readonly record struct Triad(in Effect Effect, in ImmutableArray<Income> Incomes, in ImmutableArray<Influence> Influences);
}