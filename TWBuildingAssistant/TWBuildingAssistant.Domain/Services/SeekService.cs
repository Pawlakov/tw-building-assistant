namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public class SeekService
    : ISeekService
{
    private readonly IProvinceService provinceService;

    public SeekService(IProvinceService provinceService)
    {
        this.provinceService = provinceService;
    }

    public async Task<ImmutableArray<SeekerResult>> Seek(
        Settings settings,
        EffectSet predefinedEffect,
        ImmutableArray<BuildingLibraryEntry> buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<ProvinceState> minimalCondition,
        Func<long, Task> updateProgressMax,
        Func<long, Task> updateProgressValue)
    {
        var simulationRegions = seekerSettings.Select(x => x.Slots.Select(y => new CalculationSlot(y.Descriptor, y.Branch, y.Level, y.RegionId, y.SlotIndex)).ToArray()).ToArray();
        var seekingSlots = simulationRegions.SelectMany(x => x).Where(x => x.Branch == null || x.Level == null).ToArray();

        await updateProgressMax(seekerSettings.SelectMany(x => x.Slots).Aggregate(1L, (x, y) => x * buildingLibrary.Single(z => z.Descriptor == y.Descriptor).BuildingBranches.Length));
        await updateProgressValue(0);

        var completedCounter = 0;
        var bestCombination = new List<SeekerResult>();
        var bestWealth = 0d;

        var lastSlot = seekingSlots.LastOrDefault();
        if (lastSlot != null)
        {
            await RecursiveSeek(0);
        }

        return bestCombination.ToImmutableArray();

        async Task RecursiveSeek(int slotIndex)
        {
            var slot = seekingSlots[slotIndex];
            var options = buildingLibrary.Single(x => x.Descriptor == slot.Descriptor).BuildingBranches;
            foreach (var branchOption in options)
            {
                slot.Branch = branchOption;
                var collision = simulationRegions.Any(x => x.Where(y => y.Branch != null).GroupBy(y => y.Branch).Any(y => y.Count() > 1));
                if (!collision)
                {
                    foreach (var levelOption in branchOption.Levels)
                    {
                        slot.Level = levelOption;
                        if (slot == lastSlot)
                        {
                            var state = this.provinceService.GetState(simulationRegions.Select(x => x.Select(y => y.Level.Value)), settings, predefinedEffect);
                            if (minimalCondition(state) && state.Wealth > bestWealth)
                            {
                                bestWealth = state.Wealth;
                                bestCombination.Clear();
                                bestCombination.AddRange(seekingSlots.Select(x => new SeekerResult(x.Branch.Value, x.Level.Value, x.RegionId, x.SlotIndex)));
                            }
                        }
                        else
                        {
                            await RecursiveSeek(slotIndex + 1);
                        }
                    }
                }

                await updateProgressValue(++completedCounter);
            }
        }
    }

    private class CalculationSlot
    {
        public CalculationSlot(SlotDescriptor? descriptor, BuildingBranch? branch, BuildingLevel? level, int regionId, int slotIndex)
        {
            this.Descriptor = descriptor;
            this.Branch = branch;
            this.Level = level;
            this.RegionId = regionId;
            this.SlotIndex = slotIndex;
        }

        public SlotDescriptor? Descriptor { get; set; }

        public BuildingBranch? Branch { get; set; }

        public BuildingLevel? Level { get; set; }

        public int RegionId { get; set; }

        public int SlotIndex { get; set; }
    }
}