namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.ObjectModel;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Presentation.State;

public class RegionViewModel 
    : ViewModel
{
    public RegionViewModel(IProvinceService provinceService, ISettingsStore settingsStore, IProvinceStore provinceStore, Region region)
    {
        this.Slots = new ObservableCollection<SlotViewModel>();
        var slotIndex = 0;
        foreach (var slot in region.Slots)
        {
            this.Slots.Add(new SlotViewModel(provinceService, settingsStore, provinceStore, region.Id, slotIndex++, slot.RegionType, slot.SlotType));
        }

        foreach (var slotViewModel in this.Slots)
        {
            slotViewModel.BuildingChanged += this.BuildingChangedHandler;
        }

        this.Name = region.Name;
    }

    public ObservableCollection<SlotViewModel> Slots { get; }

    public string Name { get; }

    private async void BuildingChangedHandler(object? sender, EventArgs args)
    {
        foreach (var slot in this.Slots)
        {
            if (slot != sender)
            {
                await slot.UpdateBuildings();
            }
        }
    }
}