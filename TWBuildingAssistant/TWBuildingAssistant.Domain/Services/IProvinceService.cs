namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceService
{
    ProvinceState GetState(
        Province province,
        in Settings settings,
        Effect predefinedEffect,
        ImmutableArray<Income> predefinedIncomes,
        ImmutableArray<Influence> predefinedInfluences);

    (Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences) GetStateFromSettings(
        Province province,
        in Settings settings,
        Faction faction,
        in Climate climate,
        in Religion religion);
}