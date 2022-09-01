namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Presentation.State;
using static TWBuildingAssistant.Domain.Interface;

public class SlotViewModel
    : ViewModel
{
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;

    private NamedIdWithItemsDto selectedBuildingBranch;
    private NamedIdDto selectedBuildingLevel;

    private bool selected;

    public SlotViewModel(ISettingsStore settingsStore, IProvinceStore provinceStore, int regionId, int slotIndex, SlotDescriptorDto descriptor)
    {
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;

        this.RegionId = regionId;
        this.SlotIndex = slotIndex;
        this.Descriptor = descriptor;

        this.selected = false;
        this.BuildingBranches = new ObservableCollection<NamedIdWithItemsDto>(this.settingsStore.BuildingLibrary.Single(x => x.Descriptor.Equals(descriptor)).BuildingBranches);
        if (this.provinceStore.BuildingLevels.ContainsKey((this.RegionId, this.SlotIndex)))
        {
            var fromStore = this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)];
            if (this.BuildingBranches.Any(x => x.Id == fromStore.BuildingBranchId))
            {
                var matchingBranches = this.BuildingBranches.Where(x => x.Id == fromStore.BuildingBranchId);
                this.selectedBuildingBranch = matchingBranches.Single(x => x.Items.Any(y => y.Id == fromStore.BuildingLevelId));

                this.BuildingLevels = new ObservableCollection<NamedIdDto>(this.selectedBuildingBranch.Items);
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
                this.BuildingLevels = new ObservableCollection<NamedIdDto>(this.selectedBuildingBranch.Items);
                this.selectedBuildingLevel = this.BuildingLevels[0];
            }
        }
        else
        {
            this.selectedBuildingBranch = this.BuildingBranches[0];
            this.BuildingLevels = new ObservableCollection<NamedIdDto>(this.selectedBuildingBranch.Items);
            this.selectedBuildingLevel = this.BuildingLevels[0];
        }

        var correspondingResult = this.provinceStore.SeekerResults.FirstOrDefault(x => x.SlotIndex == this.SlotIndex && x.RegionId == this.RegionId);
        if (correspondingResult != default)
        {
            var matchingBranches = this.BuildingBranches.Where(x => x.Id == correspondingResult.BranchId);
            this.selectedBuildingBranch = matchingBranches.Single(x => x.Items.Any(y => y.Id == correspondingResult.LevelId));
            this.BuildingLevels = new ObservableCollection<NamedIdDto>(this.selectedBuildingBranch.Items);
            this.selectedBuildingLevel = this.BuildingLevels.Single(x => x.Id == correspondingResult.LevelId);

            this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

            this.provinceStore.SeekerResults.Remove(correspondingResult);
        }
    }

    public event EventHandler BuildingChanged;

    public ObservableCollection<NamedIdWithItemsDto> BuildingBranches { get; }

    public ObservableCollection<NamedIdDto> BuildingLevels { get; }

    public NamedIdWithItemsDto SelectedBuildingBranch
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

                this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

                this.BuildingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public NamedIdDto SelectedBuildingLevel
    {
        get => this.selectedBuildingLevel;
        set
        {
            if (this.selectedBuildingLevel != value && value != null)
            {
                this.selectedBuildingLevel = value;
                this.OnPropertyChanged(nameof(this.SelectedBuildingLevel));

                this.provinceStore.BuildingLevels[(this.RegionId, this.SlotIndex)] = (this.selectedBuildingBranch.Id, this.selectedBuildingLevel.Id);

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