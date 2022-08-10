namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Presentation.State;

public class SlotViewModel
    : ViewModel
{
    private readonly IProvinceService provinceService;
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;

    private readonly int regionId;
    private readonly int slotIndex;
    private readonly RegionType regionType;
    private readonly SlotType slotType;

    private NamedId selectedBuildingBranch;
    private NamedId selectedBuildingLevel;

    private bool selected;

    public SlotViewModel(IProvinceService provinceService, ISettingsStore settingsStore, IProvinceStore provinceStore, int regionId, int slotIndex, RegionType regionType, SlotType slotType)
    {
        this.provinceService = provinceService;
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;

        this.regionId = regionId;
        this.slotIndex = slotIndex;
        this.regionType = regionType;
        this.slotType = slotType;

        this.selected = false;
        this.BuildingBranches = new ObservableCollection<NamedId>(this.provinceService.GetBuildingBranchOptions(this.settingsStore.Settings, this.regionType, this.slotType).Result);
        if (this.provinceStore.BuildingLevelIds.ContainsKey((this.regionId, this.slotIndex)))
        {
            var fromStore = this.provinceStore.BuildingLevelIds[(this.regionId, this.slotIndex)];
            if (this.BuildingBranches.Any(x => x.Id == fromStore.BuildingBranchId))
            {
                this.selectedBuildingBranch = this.BuildingBranches.Single(x => x.Id == fromStore.BuildingBranchId);
                this.BuildingLevels = new ObservableCollection<NamedId>(this.provinceService.GetBuildingLevelOptions(this.settingsStore.Settings, this.selectedBuildingBranch.Id).Result);
                if (this.BuildingLevels.Any(x => x.Id == fromStore.BuildingLevelId))
                {
                    this.selectedBuildingLevel = this.BuildingLevels.Single(x => x.Id == fromStore.BuildingLevelId);
                }
            }
        }
        else
        {
            this.selectedBuildingBranch = this.BuildingBranches[0];
            this.BuildingLevels = new ObservableCollection<NamedId>(this.provinceService.GetBuildingLevelOptions(this.settingsStore.Settings, this.selectedBuildingBranch.Id).Result);
            this.selectedBuildingLevel = this.BuildingLevels[0];
        }
    }

    public event EventHandler BuildingChanged;

    public ObservableCollection<NamedId> BuildingBranches { get; }

    public ObservableCollection<NamedId> BuildingLevels { get; }

    public NamedId SelectedBuildingBranch
    {
        get => this.selectedBuildingBranch;
        set
        {
            if (this.selectedBuildingBranch != value)
            {
                this.selectedBuildingBranch = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingBranch));

                this.BuildingLevels.Clear();
                foreach (var level in this.provinceService.GetBuildingLevelOptions(this.settingsStore.Settings, value.Id).Result)
                {
                    this.BuildingLevels.Add(level);
                }

                this.selectedBuildingLevel = this.BuildingLevels[0];
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevelIds[(this.regionId, this.slotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public NamedId SelectedBuildingLevel
    {
        get => this.selectedBuildingLevel;
        set
        {
            if (this.selectedBuildingLevel != value)
            {
                this.selectedBuildingLevel = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevelIds[(this.regionId, this.slotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool Selected
    {
        get => this.selected;
        set
        {
            if (this.selected != value)
            {
                this.selected = value;
                this.OnPropertyChanged(nameof(this.Selected));
            }
        }
    }

    public async Task UpdateBuildings()
    {
        this.BuildingBranches.Clear();
        foreach (var branch in await this.provinceService.GetBuildingBranchOptions(this.settingsStore.Settings, this.regionType, this.slotType))
        {
            this.BuildingBranches.Add(branch);
        }
    }
}