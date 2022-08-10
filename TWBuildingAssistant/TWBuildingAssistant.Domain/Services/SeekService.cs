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
        in ImmutableArray<Faction> factions,
        in ImmutableArray<Climate> climates,
        in ImmutableArray<Religion> religions,
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

        var faction = factions.Single(x => x.Id == settings.FactionId);
        var climate = climates.Single(x => x.Id == province.ClimateId);
        var religion = religions.Single(x => x.Id == settings.ReligionId);
        var predefinedState = this.provinceService.GetStateFromSettings(province, settings, faction, climate, religion);

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
                    var state = this.provinceService.GetState(province, settings, predefinedState.Effect, predefinedState.Incomes, predefinedState.Influences);
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