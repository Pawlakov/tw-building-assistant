﻿namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Presentation.Extensions;
using TWBuildingAssistant.Presentation.State;

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

        var province = Domain.Province.getProvince(this.configuration.GetSettings().ProvinceId);
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
                this.OnPropertyChanged(nameof(this.performance));
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
            .Select(x =>
            {
                return new Seeker.SeekerSettingsRegion(
                    x.Slots
                        .Where(y => y.SelectedBuildingBranch.Id > 0 || y.Selected)
                        .Select(y => new Seeker.SeekerSettingsSlot(y.Selected ? Microsoft.FSharp.Core.FSharpOption<Buildings.BuildingBranch>.None : y.SelectedBuildingBranch, y.Selected ? Microsoft.FSharp.Core.FSharpOption<Buildings.BuildingLevel>.None : y.SelectedBuildingLevel, y.Descriptor, y.RegionId, y.SlotIndex))
                        .ToArray());
            })
            .ToArray();

        this.navigator.CurrentViewType = INavigator.ViewType.Seeker;
    }

    private void SetPerformanceDisplay()
    {
        var state = State.getState(this.Regions.Select(x => x.Slots.Select(y => y.SelectedBuildingLevel)), this.configuration.GetSettings(), this.settingsStore.Effect);
        var provinceBuilder = new StringBuilder();
        provinceBuilder.AppendLine($"Total Wealth: {state.TotalWealth}");
        provinceBuilder.AppendLine($"Total Income: {state.TotalIncome}");
        provinceBuilder.AppendLine($"Total Food: {state.TotalFood}");
        provinceBuilder.AppendLine($"Public Order: {state.PublicOrder}");
        provinceBuilder.AppendLine($"Relgious Osmosis: {state.ReligiousOsmosis}");
        provinceBuilder.AppendLine($"Research Rate: +{state.ResearchRate}%");
        provinceBuilder.AppendLine($"Growth: {state.Growth}");
        this.Performance = provinceBuilder.ToString();

        var i = 0;
        foreach (var region in state.Regions)
        {
            var regionBuilder = new StringBuilder();
            regionBuilder.AppendLine($"Sanitation: {region.Sanitation}");
            regionBuilder.AppendLine($"Food: {region.Food}");
            regionBuilder.AppendLine($"Wealth: {region.Wealth}");
            regionBuilder.AppendLine($"Maintenance: {region.Maintenance}");
            this.Regions[i++].Performance = regionBuilder.ToString();
        }
    }
}