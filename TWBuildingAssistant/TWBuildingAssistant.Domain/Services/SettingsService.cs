namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public class SettingsService
    : ISettingsService
{
    private readonly DatabaseContextFactory contextFactory;

    public SettingsService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry>> GetBuildingLibrary(Data.FSharp.Models.Settings settings)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var slotTypes = new[] { Data.FSharp.Models.SlotType.Main, Data.FSharp.Models.SlotType.Coastal, Data.FSharp.Models.SlotType.General, };
            var regionTypes = new[] { Data.FSharp.Models.RegionType.City, Data.FSharp.Models.RegionType.Town, };
            var resourceIdsInProvince = await context.Regions
                .AsNoTracking()
                .Where(x => x.ProvinceId == settings.ProvinceId)
                .Select(x => x.ResourceId)
                .Distinct()
                .ToListAsync();

            var results = new List<Data.FSharp.Models.BuildingLibraryEntry>();
            foreach (var descriptor in slotTypes.SelectMany(x => regionTypes.SelectMany(y => resourceIdsInProvince.Select(z => new Data.FSharp.Models.SlotDescriptor(x, y, z)))))
            {
                results.Add(Data.FSharp.Buildings.getBuildingLibraryEntry(settings, descriptor));
            }

            return results.ToImmutableArray();
        }
    }
}