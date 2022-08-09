namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.OldModels;

public interface IProvinceStore
{
    List<BuildingSlot> Slots { get; set; }
}