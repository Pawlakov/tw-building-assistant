namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Models;

public interface IWorldDataService
{
    IEnumerable<Province> Provinces { get; }

    IEnumerable<Faction> Factions { get; }

    IEnumerable<Weather> GetWeathers();

    IEnumerable<Season> GetSeasons();

    IEnumerable<Religion> GetReligions();
}