namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsService
{
    Task<ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry>> GetBuildingLibrary(Data.FSharp.Models.Settings settings);
}