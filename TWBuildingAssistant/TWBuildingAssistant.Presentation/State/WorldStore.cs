namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.Services;

internal class WorldStore
    : IWorldStore
{
    private readonly IWorldDataService worldDataService;

    private ImmutableArray<Province>? provinces;
    private ImmutableArray<Faction>? factions;

    public WorldStore(IWorldDataService worldDataService)
    {
        this.worldDataService = worldDataService;
    }

    public async Task<ImmutableArray<Province>> GetProvinces()
    {
        if (this.provinces == null)
        {
            this.provinces = this.worldDataService.GetProvinces().ToImmutableArray();
        }

        return this.provinces.Value;
    }

    public async Task<ImmutableArray<Faction>> GetFactions()
    {
        if (this.factions == null)
        {
            this.factions = this.worldDataService.GetFactions().ToImmutableArray();
        }

        return this.factions.Value;
    }
}