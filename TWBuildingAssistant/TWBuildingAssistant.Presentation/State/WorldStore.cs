﻿namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.Services;

internal class WorldStore
    : IWorldStore
{
    private readonly IWorldDataService worldDataService;

    private ImmutableArray<Season>? seasons;
    private ImmutableArray<Weather>? weathers;
    private ImmutableArray<Climate>? climates;
    private ImmutableArray<Religion>? religions;
    private ImmutableArray<Province>? provinces;
    private ImmutableArray<Faction>? factions;

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

    public async Task<ImmutableArray<Climate>> GetClimates()
    {
        if (this.climates == null)
        {
            this.climates = this.worldDataService.GetClimates().ToImmutableArray();
        }

        return this.climates.Value;
    }

    public async Task<ImmutableArray<Religion>> GetReligions()
    {
        if (this.religions == null)
        {
            this.religions = this.worldDataService.GetReligions().ToImmutableArray();
        }

        return this.religions.Value;
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