﻿namespace TWBuildingAssistant.Actor.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Actor.Extensions;
using TWBuildingAssistant.Actor.State;
using static TWBuildingAssistant.Domain.DTOs;
using static TWBuildingAssistant.Domain.Interface;

public class ProvinceViewModel
: ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;
    private readonly IConfiguration configuration;

    private string performance;

    public ProvinceViewModel(INavigator navigator, ISettingsStore settingsStore, IProvinceStore provinceStore, IConfiguration configuration)
    {
        this.navigator = navigator;
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;
        this.configuration = configuration;

        var province = getProvince(this.configuration.GetSettings().ProvinceId);
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
                this.OnPropertyChanged(nameof(this.performance));
            }
        }
    }

    public RelayCommand PreviousCommand { get; init; }

    public void Previous()
    {
        this.navigator.CurrentViewType = INavigator.ViewType.Settings;
    }

    private void SetPerformanceDisplay()
    {
        var state = getState(this.Regions.Select(x => x.Slots.Select(y => y.SelectedBuildingLevel.Id).ToArray()).ToArray(), this.configuration.GetSettings());
        var provinceBuilder = new StringBuilder();
        provinceBuilder.AppendLine($"Total Wealth: {state.TotalWealth}");
        provinceBuilder.AppendLine($"Tax Rate: {state.TaxRate}%");
        provinceBuilder.AppendLine($"Corruption Rate: {state.CorruptionRate}%");
        provinceBuilder.AppendLine($"Total Income: {state.TotalIncome}");
        provinceBuilder.AppendLine($"Total Food: {state.TotalFood}");
        provinceBuilder.AppendLine($"Public Order: {state.PublicOrder}");
        provinceBuilder.AppendLine($"Relgious Osmosis: {state.ReligiousOsmosis}");
        provinceBuilder.AppendLine($"Research Rate: +{state.ResearchRate}%");
        provinceBuilder.Append($"Growth: {state.Growth}");
        this.Performance = provinceBuilder.ToString();

        var i = 0;
        foreach (var region in state.Regions)
        {
            var regionBuilder = new StringBuilder();
            regionBuilder.AppendLine($"Sanitation: {region.Sanitation}");
            regionBuilder.AppendLine($"Food: {region.Food}");
            regionBuilder.AppendLine($"Wealth: {region.Wealth}");
            regionBuilder.AppendLine($"Capital Tier: {region.CapitalTier}");
            regionBuilder.Append($"Maintenance: {region.Maintenance}");
            this.Regions[i++].Performance = regionBuilder.ToString();
        }
    }
}
