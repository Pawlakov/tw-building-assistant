namespace TWBuildingAssistant.Domain.State;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Models;

public interface IWorldStore
{
    Task<ImmutableArray<Weather>> GetWeathers();

    Task<ImmutableArray<Season>> GetSeasons();

    Task<ImmutableArray<Religion>> GetReligions();
}