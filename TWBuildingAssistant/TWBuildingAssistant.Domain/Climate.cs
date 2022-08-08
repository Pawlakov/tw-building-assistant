namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public readonly record struct Climate(int Id, string Name, ImmutableArray<ClimateSeason> Effects);

public readonly record struct ClimateSeason(int SeasonId, ImmutableArray<ClimateSeasonWeather> Effects);

public readonly record struct ClimateSeasonWeather(int WeatherId, Effect Effect, ImmutableArray<Income> Incomes);