namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

public class ProvinceStore
    : IProvinceStore
{
    public List<BuildingSlot> Slots { get; set; }
}