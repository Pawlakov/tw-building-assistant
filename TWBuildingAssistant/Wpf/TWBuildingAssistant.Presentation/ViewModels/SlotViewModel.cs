namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using TWBuildingAssistant.Presentation.State;
using static TWBuildingAssistant.Domain.DTOs;

public class SlotViewModel
    : ViewModel
{
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;

    private NamedStringIdWithItemsDto selectedBuildingBranch;
    private NamedStringIdDto selectedBuildingLevel;

    private bool selected;

    public SlotViewModel(ISettingsStore settingsStore, IProvinceStore provinceStore, int regionId, int slotIndex, SlotDescriptorDto descriptor)
    {
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;

        this.RegionId = regionId;
        this.SlotIndex = slotIndex;
        this.Descriptor = descriptor;

        this.selected = false;
        this.BuildingBranches = new ObservableCollection<NamedStringIdWithItemsDto>(this.settingsStore.BuildingLibrary.Single(x => x.Descriptor.Equals(descriptor)).BuildingBranches);
        if (this.provinceStore.BuildingLevels.ContainsKey((this.RegionId, this.SlotIndex)))
        {
            var fromStore = this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)];
            if (this.BuildingBranches.Any(x => x.StringId == fromStore.BuildingBranchId))
            {
                var matchingBranches = this.BuildingBranches.Where(x => x.StringId == fromStore.BuildingBranchId);
                this.selectedBuildingBranch = matchingBranches.Single(x => x.Items.Any(y => y.StringId == fromStore.BuildingLevelId));

                this.BuildingLevels = new ObservableCollection<NamedStringIdDto>(this.selectedBuildingBranch.Items);
                if (this.BuildingLevels.Any(x => x.StringId == fromStore.BuildingLevelId))
                {
                    this.selectedBuildingLevel = this.BuildingLevels.Single(x => x.StringId == fromStore.BuildingLevelId);
                }
                else
                {
                    this.selectedBuildingLevel = this.BuildingLevels[0];
                }
            }
            else
            {
                this.selectedBuildingBranch = this.BuildingBranches[0];
                this.BuildingLevels = new ObservableCollection<NamedStringIdDto>(this.selectedBuildingBranch.Items);
                this.selectedBuildingLevel = this.BuildingLevels[0];
            }
        }
        else
        {
            this.selectedBuildingBranch = this.BuildingBranches[0];
            this.BuildingLevels = new ObservableCollection<NamedStringIdDto>(this.selectedBuildingBranch.Items);
            this.selectedBuildingLevel = this.BuildingLevels[0];
        }

        var correspondingResult = this.provinceStore.SeekerResults.FirstOrDefault(x => x.SlotIndex == this.SlotIndex && x.RegionId == this.RegionId);
        if (correspondingResult != default)
        {
            var matchingBranches = this.BuildingBranches.Where(x => x.StringId == correspondingResult.BranchId);
            this.selectedBuildingBranch = matchingBranches.Single(x => x.Items.Any(y => y.StringId == correspondingResult.LevelId));
            this.BuildingLevels = new ObservableCollection<NamedStringIdDto>(this.selectedBuildingBranch.Items);
            this.selectedBuildingLevel = this.BuildingLevels.Single(x => x.StringId == correspondingResult.LevelId);

            this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.StringId, this.selectedBuildingLevel.StringId);

            this.provinceStore.SeekerResults.Remove(correspondingResult);
        }
    }

    public event EventHandler BuildingChanged;

    public ObservableCollection<NamedStringIdWithItemsDto> BuildingBranches { get; }

    public ObservableCollection<NamedStringIdDto> BuildingLevels { get; }

    public NamedStringIdWithItemsDto SelectedBuildingBranch
    {
        get => this.selectedBuildingBranch;
        set
        {
            if (this.selectedBuildingBranch != value && value != null)
            {
                this.selectedBuildingBranch = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingBranch));

                this.BuildingLevels.Clear();
                foreach (var level in this.selectedBuildingBranch.Items)
                {
                    this.BuildingLevels.Add(level);
                }

                this.selectedBuildingLevel = this.BuildingLevels[0];
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.StringId, this.selectedBuildingLevel.StringId);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public NamedStringIdDto SelectedBuildingLevel
    {
        get => this.selectedBuildingLevel;
        set
        {
            if (this.selectedBuildingLevel != value && value != null)
            {
                this.selectedBuildingLevel = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.StringId, this.selectedBuildingLevel.StringId);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public SlotDescriptorDto Descriptor { get; init; }

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