namespace TWBuildingAssistant.Domain;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain.Exceptions;

public static class ClimateOperations
{
    public static Climate Create(in int id, in string name, IEnumerable<(int SeasonId, IEnumerable<(int WeatherId, Effect Effect, IEnumerable<Income> Incomes)> Content)> effects)
    {
        return (id, name, effects) switch
        {
            (0, _, _) =>
                throw new DomainRuleViolationException("Climate without id."),
            (_, null or "", _) =>
                throw new DomainRuleViolationException("Climate without name."),
            _ =>
                new Climate(id, name, effects?.Select(x => Create(x.SeasonId, x.Content))?.ToImmutableArray() ?? default),
        };
    }

    public static (Effect Effect, ImmutableArray<Income> Incomes) GetEffects(in Climate climate, in int seasonId, in int weatherId)
    {
        return seasonId switch
        {
            0 =>
                throw new DomainRuleViolationException("Season id is missing."),
            var validSeasonId when climate.Effects.Any(x => x.SeasonId == validSeasonId) =>
                GetEffects(climate.Effects.Single(x => x.SeasonId == validSeasonId), weatherId),
            _ =>
                (default, default),
        };
    }

    private static ClimateSeason Create(in int seasonId, IEnumerable<(int WeatherId, Effect Effect, IEnumerable<Income> Incomes)> effects)
    {
        return (seasonId, effects) switch
        {
            (0, _) =>
                throw new DomainRuleViolationException("Season id is missing."),
            _ =>
                new ClimateSeason(seasonId, effects?.Select(x => Create(x.WeatherId, x.Effect, x.Incomes))?.ToImmutableArray() ?? default),
        };
    }

    private static ClimateSeasonWeather Create(in int weatherId, in Effect effect, IEnumerable<Income> incomes)
    {
        return weatherId switch
        {
            0 =>
                throw new DomainRuleViolationException("Weather id is missing."),
            _ =>
                new ClimateSeasonWeather(weatherId, effect, incomes?.ToImmutableArray() ?? default),
        };
    }

    private static (Effect Effect, ImmutableArray<Income> Incomes) GetEffects(in ClimateSeason climateSeason, in int weatherId)
    {
        return weatherId switch
        {
            0 =>
                throw new DomainRuleViolationException("Weather id is missing."),
            var validWeatherId when climateSeason.Effects.Any(x => x.WeatherId == validWeatherId) =>
                GetEffects(climateSeason.Effects.Single(x => x.WeatherId == validWeatherId)),
            _ =>
                (default, default),
        };
    }

    private static (Effect Effect, ImmutableArray<Income> Incomes) GetEffects(in ClimateSeasonWeather climateSeasonWeather)
    {
        return (climateSeasonWeather.Effect, climateSeasonWeather.Incomes);
    }
}