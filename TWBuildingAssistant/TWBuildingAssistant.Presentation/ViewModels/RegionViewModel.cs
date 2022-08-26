﻿namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using TWBuildingAssistant.Presentation.State;

public class RegionViewModel 
    : ViewModel
{
    public RegionViewModel(ISettingsStore settingsStore, IProvinceStore provinceStore, Domain.Models.Region region)
    {
        this.Slots = new ObservableCollection<SlotViewModel>();
        var slotIndex = 0;
        foreach (var slot in region.Slots)
        {
            this.Slots.Add(new SlotViewModel(settingsStore, provinceStore, region.Id, slotIndex++, slot));
        }

        if (region.ResourceId == null)
        {
            this.Name = region.Name;
        }
        else
        {
            this.Name = $"{region.Name} ({region.ResourceName.Value})";
        }
    }

    public ObservableCollection<SlotViewModel> Slots { get; }

    public string Name { get; }
}