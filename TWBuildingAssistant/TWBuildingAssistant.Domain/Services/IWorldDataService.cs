namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.Models;

public interface IWorldDataService
{
    IEnumerable<Religion> Religions { get; }

    IEnumerable<Province> Provinces { get; }

    IEnumerable<Faction> Factions { get;  }

    IEnumerable<Weather> Weathers { get; }

    IEnumerable<Season> Seasons { get; }
}