namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.OldModels;

public interface IWorldDataService
{
    IEnumerable<Faction> GetFactions();

    IEnumerable<Province> GetProvinces();
}