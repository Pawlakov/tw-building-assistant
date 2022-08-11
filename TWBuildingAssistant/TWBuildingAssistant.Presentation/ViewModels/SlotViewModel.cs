namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Presentation.State;

public class SlotViewModel
    : ViewModel
{
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;

    private BuildingBranch selectedBuildingBranch;
    private BuildingLevel selectedBuildingLevel;

    private bool selected;

    public SlotViewModel(ISettingsStore settingsStore, IProvinceStore provinceStore, int regionId, int slotIndex, SlotDescriptor descriptor)
    {
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;

        this.RegionId = regionId;
        this.SlotIndex = slotIndex;
        this.Descriptor = descriptor;

        this.selected = false;
        this.BuildingBranches = new ObservableCollection<BuildingBranch>(this.settingsStore.BuildingLibrary.Single(x => x.Descriptor == descriptor).BuildingBranches);
        if (this.provinceStore.BuildingLevelIds.ContainsKey((this.RegionId, this.SlotIndex)))
        {
            var fromStore = this.provinceStore.BuildingLevelIds[(this.RegionId, this.SlotIndex)];
            if (this.BuildingBranches.Any(x => x.Id == fromStore.BuildingBranchId))
            {
                var matchingBranches = this.BuildingBranches.Where(x => x.Id == fromStore.BuildingBranchId);
                this.selectedBuildingBranch = matchingBranches.Single(x => x.Levels.Any(y => y.Id == fromStore.BuildingLevelId));

                this.BuildingLevels = new ObservableCollection<BuildingLevel>(this.selectedBuildingBranch.Levels);
                if (this.BuildingLevels.Any(x => x.Id == fromStore.BuildingLevelId))
                {
                    this.selectedBuildingLevel = this.BuildingLevels.Single(x => x.Id == fromStore.BuildingLevelId);
                }
                else
                {
                    this.selectedBuildingLevel = this.BuildingLevels[0];
                }
            }
            else
            {
                this.selectedBuildingBranch = this.BuildingBranches[0];
                this.BuildingLevels = new ObservableCollection<BuildingLevel>(this.selectedBuildingBranch.Levels);
                this.selectedBuildingLevel = this.BuildingLevels[0];
            }
        }
        else
        {
            this.selectedBuildingBranch = this.BuildingBranches[0];
            this.BuildingLevels = new ObservableCollection<BuildingLevel>(this.selectedBuildingBranch.Levels);
            this.selectedBuildingLevel = this.BuildingLevels[0];
        }

        var correspondingResult = this.provinceStore.SeekerResults.FirstOrDefault(x => x.SlotIndex == this.SlotIndex && x.RegionId == this.RegionId);
        if (correspondingResult != default)
        {
            this.selectedBuildingBranch = correspondingResult.Branch;
            this.BuildingLevels = new ObservableCollection<BuildingLevel>(this.selectedBuildingBranch.Levels);
            this.selectedBuildingLevel = correspondingResult.Level;

            this.provinceStore.SeekerResults.Remove(correspondingResult);
        }
    }

    public event EventHandler BuildingChanged;

    public ObservableCollection<BuildingBranch> BuildingBranches { get; }

    public ObservableCollection<BuildingLevel> BuildingLevels { get; }

    public BuildingBranch SelectedBuildingBranch
    {
        get => this.selectedBuildingBranch;
        set
        {
            if (this.selectedBuildingBranch != value)
            {
                this.selectedBuildingBranch = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingBranch));

                this.BuildingLevels.Clear();
                foreach (var level in this.selectedBuildingBranch.Levels)
                {
                    this.BuildingLevels.Add(level);
                }

                this.selectedBuildingLevel = this.BuildingLevels[0];
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevelIds[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public BuildingLevel SelectedBuildingLevel
    {
        get => this.selectedBuildingLevel;
        set
        {
            if (this.selectedBuildingLevel != value)
            {
                this.selectedBuildingLevel = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevelIds[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public SlotDescriptor Descriptor { get; init; }

    public int RegionId { get; init; }

    public int SlotIndex { get; init; }

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

    /*public async Task UpdateBuildings()
    {
        this.BuildingBranches.Clear();
        foreach (var branch in await this.provinceService.GetBuildingBranchOptions(this.settingsStore.Settings, this.regionType, this.slotType))
        {
            this.BuildingBranches.Add(branch);
        }
    }*/
}