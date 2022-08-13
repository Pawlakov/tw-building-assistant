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
        if (this.provinceStore.BuildingLevels.ContainsKey((this.RegionId, this.SlotIndex)))
        {
            var fromStore = this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)];
            if (this.BuildingBranches.Any(x => x == fromStore.BuildingBranch))
            {
                var matchingBranches = this.BuildingBranches.Where(x => x == fromStore.BuildingBranch);
                this.selectedBuildingBranch = matchingBranches.Single(x => x.Levels.Any(y => y == fromStore.BuildingLevel));

                this.BuildingLevels = new ObservableCollection<BuildingLevel>(this.selectedBuildingBranch.Levels);
                if (this.BuildingLevels.Any(x => x == fromStore.BuildingLevel))
                {
                    this.selectedBuildingLevel = this.BuildingLevels.Single(x => x == fromStore.BuildingLevel);
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

            this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch, this.selectedBuildingLevel);

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
            if (this.selectedBuildingBranch != value && value != null)
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

                this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch, this.selectedBuildingLevel);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public BuildingLevel SelectedBuildingLevel
    {
        get => this.selectedBuildingLevel;
        set
        {
            if (this.selectedBuildingLevel != value && value != null)
            {
                this.selectedBuildingLevel = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch, this.selectedBuildingLevel);

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
}