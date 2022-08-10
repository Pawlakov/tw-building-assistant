namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsService
{
    Task<IEnumerable<NamedId>> GetWeatherOptions();

    Task<IEnumerable<NamedId>> GetSeasonOptions();

    Task<IEnumerable<NamedId>> GetReligionOptions();

    Task<IEnumerable<NamedId>> GetProvinceOptions();

    Task<IEnumerable<NamedId>> GetFactionOptions();

    Task<(Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences)> GetStateFromSettings(Settings settings);
}