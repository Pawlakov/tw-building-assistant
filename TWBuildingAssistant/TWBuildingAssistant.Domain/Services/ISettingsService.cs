namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsService
{
    Task<EffectSet> GetStateFromSettings(Settings settings);

    Task<ImmutableArray<BuildingLibraryEntry>> GetBuildingLibrary(Settings settings);
}