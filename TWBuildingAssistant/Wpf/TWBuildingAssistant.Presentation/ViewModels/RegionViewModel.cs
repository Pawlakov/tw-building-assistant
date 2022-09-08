namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using TWBuildingAssistant.Presentation.State;
using static TWBuildingAssistant.Domain.DTOs;

public class RegionViewModel 
    : ViewModel
{
    private string performance;

    public RegionViewModel(ISettingsStore settingsStore, IProvinceStore provinceStore, RegionDto region)
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

    public string Performance
    {
        get => this.performance;
        set
        {
            if (this.performance != value)
            {
                this.performance = value;
                this.OnPropertyChanged(nameof(this.performance));
            }
        }
    }
}