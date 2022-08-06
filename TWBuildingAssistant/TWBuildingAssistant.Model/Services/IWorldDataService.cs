namespace TWBuildingAssistant.Model.Services;

using System.Collections.Generic;

public interface IWorldDataService
{
    IEnumerable<Religion> Religions { get; }

    IEnumerable<Province> Provinces { get; }

    IEnumerable<Faction> Factions { get;  }

    IEnumerable<Weather> Weathers { get; }

    IEnumerable<Season> Seasons { get; }
}