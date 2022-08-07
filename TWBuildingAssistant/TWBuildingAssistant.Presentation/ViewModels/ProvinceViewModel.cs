﻿namespace TWBuildingAssistant.Presentation.ViewModels;

using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Models;

public class ProvinceViewModel 
    : ViewModel
{
    private readonly Province province;

    private string performance;

    public ProvinceViewModel(Province province)
    {
        this.province = province;
        this.ProvinceName = this.province.Name;
        this.Regions = new ObservableCollection<RegionViewModel>();
        foreach (var region in this.province.Regions)
        {
            var newRegion = new RegionViewModel(this.province, region);
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

    public event EventHandler<EventArgs> PreviousTransition;

    public event EventHandler<NextTransitionEventArgs> NextTransition;

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

    public ProvinceState CurrentState => this.province.State;

    public RelayCommand PreviousCommand { get; init; }

    public RelayCommand NextCommand { get; init; }

    public void Previous()
    {
        this.PreviousTransition?.Invoke(this, EventArgs.Empty);
    }

    public void Next()
    {
        var slots = this.Regions.SelectMany(x => x.Slots.Where(y => y.Seek)).Select(y => y.Slot).ToList();
        this.NextTransition?.Invoke(this, new NextTransitionEventArgs(this.province, slots));
    }

    private void SetPerformanceDisplay()
    {
        var state = this.province.State;
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

    public class NextTransitionEventArgs : EventArgs
    {
        public NextTransitionEventArgs(Province province, IEnumerable<BuildingSlot> slots)
        {
            this.Province = province;
            this.Slots = slots.ToList();
        }

        public Province Province { get; }

        public IEnumerable<BuildingSlot> Slots { get; }
    }
}