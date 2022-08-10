namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.OldModels;

public interface IWorldStore
{
    Task<ImmutableArray<Province>> GetProvinces();

    Task<ImmutableArray<Faction>> GetFactions();
}