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
    public ImmutableArray<SeekerResult> Seek(
        Data.FSharp.Models.Settings settings,
        Data.FSharp.Models.EffectSet predefinedEffect,
        Data.FSharp.Models.BuildingLibraryEntry[] buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<Data.FSharp.Models.ProvinceState> minimalCondition,
        Func<long, Task> updateProgressMax,
        Func<long, Task> updateProgressValue)
    {
        updateProgressMax(0);
        updateProgressValue(0);

        var combinations = this.GetCombinationsToSeek(buildingLibrary, seekerSettings);
        updateProgressMax(combinations.Length);

        var completedCounter = 0;

        SeekerResultWithWealth? Loop(CombinationTask combination)
        {
            SeekerResultWithWealth? RecursiveSeek(int regionIndex, int slotIndex, IEnumerable<IEnumerable<SeekerResult>> combinationResult, IEnumerable<SeekerResult> combinationRegionResult)
            {
                if (regionIndex < combination.Regions.Length)
                {
                    if (slotIndex < combination.Regions[regionIndex].Slots.Length)
                    {
                        var seekResult = Enumerable.Empty<object>();
                        var slot = combination.Regions[regionIndex].Slots[slotIndex];
                        if (slot.Level == null)
                        {
                            var subResults = new List<SeekerResultWithWealth?>();
                            foreach (var levelOption in slot.Branch.Levels)
                            {
                                subResults.Add(RecursiveSeek(regionIndex, slotIndex + 1, combinationResult, combinationRegionResult.Append(new SeekerResult(slot.Branch, levelOption, slot.RegionId, slot.SlotIndex))));
                            }

                            return subResults
                                .Where(x => x != null)
                                .OrderByDescending(x => x.Wealth)
                                .FirstOrDefault();
                        }
                        else
                        {
                            return RecursiveSeek(regionIndex, slotIndex + 1, combinationResult, combinationRegionResult.Append(new SeekerResult(slot.Branch, slot.Level, slot.RegionId, slot.SlotIndex)));
                        }
                    }
                    else
                    {
                        return RecursiveSeek(regionIndex + 1, 0, combinationResult.Append(combinationRegionResult), new List<SeekerResult>());
                    }
                }
                else
                {
                    var state = Data.FSharp.State.getState(combinationResult.Select(x => x.Select(y => y.Level)), settings, predefinedEffect);
                    if (minimalCondition(state))
                    {
                        return new SeekerResultWithWealth(state.Wealth, combinationResult.SelectMany(x => x).ToImmutableArray());
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            var loopResult = RecursiveSeek(0, 0, new List<List<SeekerResult>>(), new List<SeekerResult>());

            lock (updateProgressValue)
            {
                updateProgressValue(++completedCounter);
            }

            return loopResult;
        }

        var bestCombination = combinations
            .AsParallel()
            .Select(Loop)
            .Where(x => x != null)
            .OrderByDescending(x => x.Wealth)
            .FirstOrDefault()
            ?.Result;

        return bestCombination ?? Enumerable.Empty<SeekerResult>().ToImmutableArray();
    }

    private ImmutableArray<CombinationTask> GetCombinationsToSeek(Data.FSharp.Models.BuildingLibraryEntry[] buildingLibrary, ImmutableArray<SeekerSettingsRegion> seekerSettings)
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
                return new[] { new CombinationTask(combination.Select(x => new CombinationTaskRegion(x.Slots.Select(y => new CalculationSlot(y.Descriptor, y.Branch, y.Level, y.RegionId, y.SlotIndex)).ToImmutableArray())).ToImmutableArray()) };
            }
            else
            {
                var regionOptions = regionCombinations[regionIndex];
                return regionOptions.Select(x => RecursiveSeek(regionIndex + 1, combination.Append(x))).SelectMany(x => x);
            }
        }
    }

    private ImmutableArray<CombinationTaskRegion> GetRegionCombinationsToSeek(Data.FSharp.Models.BuildingLibraryEntry[] buildingLibrary, SeekerSettingsRegion regionSeekerSettings)
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
                    return slotOptions.Select(x => RecursiveSeek(slotIndex + 1, combination.Append(new CalculationSlot(slot.Descriptor, x, null, slot.RegionId, slot.SlotIndex)))).SelectMany(x => x);
                }
            }
        }
    }

    private record class CombinationTask(ImmutableArray<CombinationTaskRegion> Regions);

    private record class CombinationTaskRegion(ImmutableArray<CalculationSlot> Slots);

    private record class SeekerResultWithWealth(double Wealth, ImmutableArray<SeekerResult> Result);

    private record class CalculationSlot(Data.FSharp.Models.SlotDescriptor Descriptor, Data.FSharp.Models.BuildingBranch? Branch, Data.FSharp.Models.BuildingLevel? Level, int RegionId, int SlotIndex);
}