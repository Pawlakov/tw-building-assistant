namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsService
{
    Task<ImmutableArray<BuildingLibraryEntry>> GetBuildingLibrary(Data.FSharp.Models.Settings settings);
}