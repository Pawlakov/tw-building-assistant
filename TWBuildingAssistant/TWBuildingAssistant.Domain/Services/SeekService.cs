namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain.Models;

public class SeekService
    : ISeekService
{
    public void Seek(Province province, List<BuildingSlot> slots, Predicate<ProvinceState> minimalCondition, Action<int> updateProgressMax, Action<int> updateProgressValue)
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
            var options = province.Owner.GetBuildingLevelsForSlot(province, province.Regions.Single(x => x.Slots.Contains(slot)), slot);
            foreach (var option in options)
            {
                slot.Building = option;
                var currentCombination = combination.Append(option);
                if (slot == lastSlot)
                {
                    var state = province.State;
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