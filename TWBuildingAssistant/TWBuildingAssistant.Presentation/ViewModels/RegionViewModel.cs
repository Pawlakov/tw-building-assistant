namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Presentation.State;

public class RegionViewModel 
    : ViewModel
{
    public RegionViewModel(ISettingsStore settingsStore, IProvinceStore provinceStore, Region region)
    {
        this.Slots = new ObservableCollection<SlotViewModel>();
        var slotIndex = 0;
        foreach (var slot in region.Slots)
        {
            this.Slots.Add(new SlotViewModel(settingsStore, provinceStore, region.Id, slotIndex++, slot));
        }

        /*foreach (var slotViewModel in this.Slots)
        {
            slotViewModel.BuildingChanged += this.BuildingChangedHandler;
        }*/

        this.Name = region.Name;
    }

    public ObservableCollection<SlotViewModel> Slots { get; }

    public string Name { get; }

    /*private async void BuildingChangedHandler(object? sender, EventArgs args)
    {
        foreach (var slot in this.Slots)
        {
            if (slot != sender)
            {
                await slot.UpdateBuildings();
            }
        }
    }*/
}