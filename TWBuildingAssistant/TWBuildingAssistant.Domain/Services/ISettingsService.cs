namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISettingsService
{
    Task<IEnumerable<NamedId>> GetWeatherOptions();

    Task<IEnumerable<NamedId>> GetSeasonOptions();

    Task<IEnumerable<NamedId>> GetReligionOptions();

    Task<IEnumerable<NamedId>> GetProvinceOptions();

    Task<IEnumerable<NamedId>> GetFactionOptions();
}