namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;

public interface ISeekService
{
    void Seek(ImmutableArray<Climate> climates, ImmutableArray<Religion> religions, Province province, List<BuildingSlot> slots, Predicate<ProvinceState> minimalCondition, Action<int> updateProgressMax, Action<int> updateProgressValue);
}