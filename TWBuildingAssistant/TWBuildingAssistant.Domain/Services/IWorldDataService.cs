namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;

public interface IWorldDataService
{
    IEnumerable<Weather> GetWeathers();

    IEnumerable<Season> GetSeasons();

    IEnumerable<Climate> GetClimates();

    IEnumerable<Religion> GetReligions();

    IEnumerable<Resource> GetResources();
}