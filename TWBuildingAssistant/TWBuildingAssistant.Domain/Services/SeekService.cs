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

    public ImmutableArray<SeekerResult> Seek(
        Data.FSharp.Models.Settings settings,
        Data.FSharp.Models.EffectSet predefinedEffect,
        ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry> buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<ProvinceState> minimalCondition,
        Func<long, Task> updateProgressMax,
        Func<long, Task> updateProgressValue)
    {
        updateProgressMax(0);
        updateProgressValue(0);

        var combinations = this.GetCombinationsToSeek(buildingLibrary, seekerSettings);
        updateProgressMax(combinations.Length);

        var completedCounter = 0;
        var bestCombination = new List<SeekerResult>();
        var bestWealth = 0d;

        Parallel.ForEach(combinations, combination =>
        {
            var seekingSlots = combination.Regions.SelectMany(x => x.Slots).Where(x => x.Level == null).ToArray();

            RecursiveSeek(0);

            lock (updateProgressValue)
            {
                updateProgressValue(++completedCounter);
            }

            void RecursiveSeek(int slotIndex)
            {
                if (slotIndex < seekingSlots.Length)
                {
                    var seekResult = Enumerable.Empty<object>();
                    var slot = seekingSlots[slotIndex];
                    foreach (var levelOption in slot.Branch.Levels)
                    {
                        slot.Level = levelOption;
                        RecursiveSeek(slotIndex + 1);
                        slot.Level = null;
                    }
                }
                else
                {
                    var state = this.provinceService.GetState(combination.Regions.Select(x => x.Slots.Select(y => y.Level)), settings, predefinedEffect);
                    if (minimalCondition(state) && state.Wealth > bestWealth)
                    {
                        lock (bestCombination)
                        {
                            bestWealth = state.Wealth;
                            bestCombination.Clear();
                            bestCombination.AddRange(seekingSlots.Select(x => new SeekerResult(x.Branch, x.Level, x.RegionId, x.SlotIndex)));
                        }
                    }
                }
            }
        });

        return bestCombination.ToImmutableArray();
    }

    private ImmutableArray<CombinationTask> GetCombinationsToSeek(ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry> buildingLibrary, ImmutableArray<SeekerSettingsRegion> seekerSettings)
    {
        var regionCombinations = new List<IEnumerable<CombinationTaskRegion>>();
        foreach (var region in seekerSettings)
        {
            regionCombinations.Add(this.GetRegionCombinationsToSeek(buildingLibrary, region));
        }

        var combinations = RecursiveSeek(0, Enumerable.Empty<CombinationTaskRegion>()).ToImmutableArray();
        return combinations;

        IEnumerable<CombinationTask> RecursiveSeek(int regionIndex, IEnumerable<CombinationTaskRegion> combination)
        {
            if (regionIndex == regionCombinations.Count)
            {
                return new[] { new CombinationTask(combination.Select(x => new CombinationTaskRegion(x.Slots.Select(x => new CalculationSlot(x)).ToImmutableArray())).ToImmutableArray()) };
            }
            else
            {
                var regionOptions = regionCombinations[regionIndex];
                return regionOptions.Select(x => RecursiveSeek(regionIndex + 1, combination.Append(x))).SelectMany(x => x);
            }
        }
    }

    private ImmutableArray<CombinationTaskRegion> GetRegionCombinationsToSeek(ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry> buildingLibrary, SeekerSettingsRegion regionSeekerSettings)
    {
        var simulationSlots = regionSeekerSettings.Slots.Select(y => new CalculationSlot(y.Descriptor, y.Branch, y.Level, y.RegionId, y.SlotIndex)).ToArray();
        var options = simulationSlots.Select(x => buildingLibrary.Single(y => y.Descriptor.Equals(x.Descriptor)).BuildingBranches).ToArray();

        var validCombinations = RecursiveSeek(0, Enumerable.Empty<CalculationSlot>()).ToImmutableArray();
        return validCombinations;

        IEnumerable<CombinationTaskRegion> RecursiveSeek(int slotIndex, IEnumerable<CalculationSlot> combination)
        {
            if (slotIndex == simulationSlots.Length)
            {
                return new[] { new CombinationTaskRegion(combination.ToImmutableArray()) };
            }
            else
            {
                var slot = simulationSlots[slotIndex];
                if (slot.Level != null)
                {
                    return RecursiveSeek(slotIndex + 1, combination.Append(slot));
                }
                else
                {
                    var slotOptions = options[slotIndex].Where(x => x.Interesting).Where(x => x.Id == 0 || simulationSlots.Where(y => y.Level != null).Concat(combination).All(y => y.Branch != x));
                    return slotOptions.Select(x => RecursiveSeek(slotIndex + 1, combination.Append(new CalculationSlot(slot, x)))).SelectMany(x => x);
                }
            }
        }
    }

    private record class CombinationTask(ImmutableArray<CombinationTaskRegion> Regions);

    private record class CombinationTaskRegion(ImmutableArray<CalculationSlot> Slots);

    private class CalculationSlot
    {
        public CalculationSlot(Data.FSharp.Models.SlotDescriptor? descriptor, Data.FSharp.Models.BuildingBranch? branch, Data.FSharp.Models.BuildingLevel? level, int regionId, int slotIndex)
        {
            this.Descriptor = descriptor;
            this.Branch = branch;
            this.Level = level;
            this.RegionId = regionId;
            this.SlotIndex = slotIndex;
        }

        public CalculationSlot(CalculationSlot original)
        {
            this.Descriptor = original.Descriptor;
            this.Branch = original.Branch;
            this.Level = original.Level;
            this.RegionId = original.RegionId;
            this.SlotIndex = original.SlotIndex;
        }

        public CalculationSlot(CalculationSlot original, Data.FSharp.Models.BuildingBranch? branch)
        {
            this.Descriptor = original.Descriptor;
            this.Branch = branch;
            this.Level = null;
            this.RegionId = original.RegionId;
            this.SlotIndex = original.SlotIndex;
        }

        public Data.FSharp.Models.SlotDescriptor? Descriptor { get; set; }

        public Data.FSharp.Models.BuildingBranch? Branch { get; set; }

        public Data.FSharp.Models.BuildingLevel? Level { get; set; }

        public int RegionId { get; set; }

        public int SlotIndex { get; set; }
    }
}