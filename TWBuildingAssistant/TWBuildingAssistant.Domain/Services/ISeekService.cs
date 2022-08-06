namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISeekService
{
    void Seek(Province province, List<BuildingSlot> slots, Predicate<ProvinceState> minimalCondition, Action<int> updateProgressMax, Action<int> updateProgressValue);
}