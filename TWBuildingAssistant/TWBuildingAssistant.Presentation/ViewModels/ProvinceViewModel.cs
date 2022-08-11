namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Domain.StateModels;
using TWBuildingAssistant.Presentation.State;

public class ProvinceViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;
    private readonly IProvinceService provinceService;

    private string performance;

    public ProvinceViewModel(INavigator navigator, ISettingsStore settingsStore, IProvinceStore provinceStore, IProvinceService provinceService)
    {
        this.navigator = navigator;
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;
        this.provinceService = provinceService;

        var province = this.provinceService.GetProvince(this.settingsStore.Settings.ProvinceId).Result;
        this.ProvinceName = province.Name;
        this.Regions = new ObservableCollection<RegionViewModel>();
        foreach (var region in province.Regions)
        {
            var newRegion = new RegionViewModel(this.settingsStore, this.provinceStore, region);
            foreach (var slot in newRegion.Slots)
            {
                slot.BuildingChanged += (sender, args) => this.SetPerformanceDisplay();
            }

            this.Regions.Add(newRegion);
        }

        this.PreviousCommand = new RelayCommand(this.Previous);
        this.NextCommand = new RelayCommand(this.Next);

        this.SetPerformanceDisplay();
    }

    public string ProvinceName { get; }

    public ObservableCollection<RegionViewModel> Regions { get; }

    public string Performance
    {
        get => this.performance;
        set
        {
            if (this.performance != value)
            {
                this.performance = value;
                this.OnPropertyChanged(nameof(this.Performance));
            }
        }
    }

    public RelayCommand PreviousCommand { get; init; }

    public RelayCommand NextCommand { get; init; }

    public void Previous()
    {
        this.navigator.CurrentViewType = INavigator.ViewType.Settings;
    }

    public void Next()
    {
        this.provinceStore.SeekerSettings = this.Regions
            .Select(x => new SeekerSettingsRegion(x.Slots.Where(y => y.SelectedBuildingBranch.Id > 0 || y.Selected).Select(y => new SeekerSettingsSlot(y.Selected ? null : y.SelectedBuildingBranch, y.Selected ? null : y.SelectedBuildingLevel, y.Descriptor, y.RegionId, y.SlotIndex)).ToImmutableArray()))
            .ToImmutableArray();

        this.navigator.CurrentViewType = INavigator.ViewType.Seeker;
    }

    private void SetPerformanceDisplay()
    {
        var state = this.provinceService.GetState(this.Regions.Select(x => x.Slots.Select(y => y.SelectedBuildingLevel)), this.settingsStore.Settings, this.settingsStore.Effect);
        var builder = new StringBuilder();
        builder.AppendLine($"Sanitation: {string.Join("/", state.Regions.Select(x => x.Sanitation.ToString()))}");
        builder.AppendLine($"Food: {state.Food}");
        builder.AppendLine($"Public Order: {state.PublicOrder}");
        builder.AppendLine($"Relgious Osmosis: {state.ReligiousOsmosis}");
        builder.AppendLine($"Research Rate: +{state.ResearchRate}%");
        builder.AppendLine($"Growth: {state.Growth}");
        builder.AppendLine($"Wealth: {state.Wealth}");
        this.Performance = builder.ToString();
    }
}