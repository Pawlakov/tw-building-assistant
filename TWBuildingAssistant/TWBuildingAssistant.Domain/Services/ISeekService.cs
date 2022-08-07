namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Models;

public interface ISeekService
{
    void Seek(Province province, List<BuildingSlot> slots, Predicate<ProvinceState> minimalCondition, Action<int> updateProgressMax, Action<int> updateProgressValue);
}