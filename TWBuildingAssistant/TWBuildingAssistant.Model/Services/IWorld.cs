namespace TWBuildingAssistant.Model.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWorld
{
    IEnumerable<Religion> Religions { get; }

    IEnumerable<Province> Provinces { get; }

    IEnumerable<Faction> Factions { get;  }

    IEnumerable<Weather> Weathers { get; }

    IEnumerable<Season> Seasons { get; }
}