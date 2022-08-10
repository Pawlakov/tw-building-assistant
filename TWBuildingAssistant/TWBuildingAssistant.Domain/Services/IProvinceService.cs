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
}