namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;

public interface IWorldStore
{
    Task<ImmutableArray<Weather>> GetWeathers();

    Task<ImmutableArray<Season>> GetSeasons();

    Task<ImmutableArray<Climate>> GetClimates();

    Task<ImmutableArray<Religion>> GetReligions();

    Task<ImmutableArray<Province>> GetProvinces();

    Task<ImmutableArray<Faction>> GetFactions();
}