namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Services;

internal class WorldStore
    : IWorldStore
{
    private readonly IWorldDataService worldDataService;

    private ImmutableArray<Season>? seasons;
    private ImmutableArray<Weather>? weathers;

    public WorldStore(IWorldDataService worldDataService)
    {
        this.worldDataService = worldDataService;
    }

    public async Task<ImmutableArray<Weather>> GetWeathers()
    {
        if (this.weathers == null)
        {
            this.weathers = this.worldDataService.GetWeathers().ToImmutableArray();
        }

        return this.weathers.Value;
    }

    public async Task<ImmutableArray<Season>> GetSeasons()
    {
        if (this.seasons == null)
        {
            this.seasons = this.worldDataService.GetSeasons().ToImmutableArray();
        }

        return this.seasons.Value;
    }
}