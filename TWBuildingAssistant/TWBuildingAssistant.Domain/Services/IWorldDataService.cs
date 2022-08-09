namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;

public interface IWorldDataService
{
    IEnumerable<Weather> GetWeathers();

    IEnumerable<Season> GetSeasons();

    IEnumerable<Climate> GetClimates();

    IEnumerable<Religion> GetReligions();

    IEnumerable<Resource> GetResources();

    IEnumerable<Faction> GetFactions();

    IEnumerable<Province> GetProvinces();
}