namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Presentation.State;

public class ProvinceViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly IWorldStore worldStore;
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;
    private readonly IProvinceService provinceService;

    private readonly Province province;
    private readonly Faction faction;
    private readonly Climate climate;
    private readonly Religion religion;

    private string performance;

    public ProvinceViewModel(INavigator navigator, IWorldStore worldStore, ISettingsStore settingsStore, IProvinceStore provinceStore, IProvinceService provinceService)
    {
        this.navigator = navigator;
        this.worldStore = worldStore;
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;
        this.provinceService = provinceService;

        this.province = this.worldStore.GetProvinces().Result.Single(x => x.Id == this.settingsStore.ProvinceId);
        this.faction = this.worldStore.GetFactions().Result.Single(x => x.Id == this.settingsStore.CurrentProvinceSettings.FactionId);
        this.climate = this.worldStore.GetClimates().Result.Single(x => x.Id == this.province.ClimateId);
        this.religion = this.worldStore.GetReligions().Result.Single(x => x.Id == this.settingsStore.CurrentFactionSettings.ReligionId);

        this.ProvinceName = this.province.Name;
        this.Regions = new ObservableCollection<RegionViewModel>();
        foreach (var region in this.province.Regions)
        {
            var newRegion = new RegionViewModel(this.settingsStore, this.faction, this.province, region);
            foreach (var slot in newRegion.Slots)
            {
                slot.PropertyChanged += (sender, args) => this.SetPerformanceDisplay();
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
        this.provinceStore.Slots = this.Regions.SelectMany(x => x.Slots.Where(y => y.Seek)).Select(y => y.Slot).ToList();

        this.navigator.CurrentViewType = INavigator.ViewType.Seeker;
    }

    private void SetPerformanceDisplay()
    {
        var provinceId = this.settingsStore.ProvinceId;
        var state = this.province.GetState(this.settingsStore.CurrentProvinceSettings, this.settingsStore.CurrentFactionSettings, this.faction, this.climate, this.religion);
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