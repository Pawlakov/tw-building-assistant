namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
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

    public void Seek(
        Settings settings,
        EffectSet predefinedEffect,
        ImmutableArray<BuildingLibraryEntry> buildingLibrary,
        ImmutableArray<SeekerSettingsRegion> seekerSettings,
        Predicate<ProvinceState> minimalCondition,
        Action<long> updateProgressMax,
        Action<long> updateProgressValue)
    {
        updateProgressMax(seekerSettings.SelectMany(x => x.Slots).Aggregate(1L, (x, y) => x * buildingLibrary.Single(z => z.Descriptor == y).BuildingBranches.Length));
        updateProgressValue(0);

        /*var lastSlot = slots.Last();
        var original = slots.Select(x => x.Building).ToList().AsEnumerable();
        var bestCombination = original.ToList().AsEnumerable();
        var bestWealth = 0d;

        RecursiveSeek(0, new List<BuildingLevel>());
        var enumerator = bestCombination.GetEnumerator();
        foreach (var slot in slots)
        {
            enumerator.MoveNext();
            slot.Building = enumerator.Current;
        }*/

        Thread.Sleep(5000);
        updateProgressValue(100);

        /*void RecursiveSeek(int slotIndex, IEnumerable<BuildingLevel> combination)
        {
            var slotDescriptor = slots[slotIndex];
            var options = buildingLibrary.Single(x => x.Descriptor == slotDescriptor).BuildingBranches;
            foreach (var option in options)
            {
                slotDescriptor.Building = option;
                var currentCombination = combination.Append(option);
                if (slotDescriptor == lastSlot)
                {
                    var state = this.provinceService.GetState(province, settings, predefinedEffect.Effect, predefinedEffect.Incomes, predefinedEffect.Influences);
                    if (minimalCondition(state) && state.Wealth > bestWealth)
                    {
                        bestWealth = state.Wealth;
                        bestCombination = currentCombination;
                    }
                }
                else
                {
                    RecursiveSeek(slotIndex + 1, currentCombination);
                }
            }
        }*/
    }
}