namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Presentation.State;

public class RegionViewModel 
    : ViewModel
{
    public RegionViewModel(ISettingsStore settingsStore, Faction faction, Province province, Region region)
    {
        this.Slots = new ObservableCollection<SlotViewModel>();
        foreach (var slot in region.Slots)
        {
            this.Slots.Add(new SlotViewModel(settingsStore, faction, region, slot));
        }

        foreach (var slotViewModel in this.Slots)
        {
            slotViewModel.BuildingChanged += (sender, args) => this.Slots.ToList().ForEach(x => x.UpdateBuildings());
            slotViewModel.UpdateBuildings();
        }

        this.Name = region.Name;
    }

    public ObservableCollection<SlotViewModel> Slots { get; }

    public string Name { get; }
}