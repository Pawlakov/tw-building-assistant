namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
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
        Effect predefinedEffect,
        ImmutableArray<Income> predefinedIncomes,
        ImmutableArray<Influence> predefinedInfluences,
        Faction faction,
        Province province,
        List<BuildingSlot> slots,
        Predicate<ProvinceState> minimalCondition,
        Action<int> updateProgressMax,
        Action<int> updateProgressValue)
    {
        var lastSlot = slots.Last();
        var original = slots.Select(x => x.Building).ToList().AsEnumerable();
        var bestCombination = original.ToList().AsEnumerable();
        var bestWealth = 0d;

        updateProgressMax(100);
        updateProgressValue(0);
        RecursiveSeek(0, new List<BuildingLevel>());
        var enumerator = bestCombination.GetEnumerator();
        foreach (var slot in slots)
        {
            enumerator.MoveNext();
            slot.Building = enumerator.Current;
        }

        updateProgressValue(100);

        void RecursiveSeek(int slotIndex, IEnumerable<BuildingLevel> combination)
        {
            var slot = slots[slotIndex];
            var options = faction.GetBuildingLevelsForSlot(settings, province.Regions.Single(x => x.Slots.Contains(slot)), slot);
            foreach (var option in options)
            {
                slot.Building = option;
                var currentCombination = combination.Append(option);
                if (slot == lastSlot)
                {
                    var state = this.provinceService.GetState(province, settings, predefinedEffect, predefinedIncomes, predefinedInfluences);
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
        }
    }
}