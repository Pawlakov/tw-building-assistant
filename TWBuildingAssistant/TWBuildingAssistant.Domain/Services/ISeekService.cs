namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

public interface ISeekService
{
    void Seek(
        ProvinceSettings provinceSettings,
        FactionSettings factionSettings,
        in ImmutableArray<Faction> factions,
        in ImmutableArray<Climate> climates,
        in ImmutableArray<Religion> religions,
        Province province,
        List<BuildingSlot> slots,
        Predicate<ProvinceState> minimalCondition,
        Action<int> updateProgressMax,
        Action<int> updateProgressValue);
}