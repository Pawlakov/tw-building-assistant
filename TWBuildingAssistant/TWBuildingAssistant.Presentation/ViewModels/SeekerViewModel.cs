﻿namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Presentation.State;

public class SeekerViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;
    private readonly ISeekService seekService;

    /*private readonly IEnumerable<BuildingSlot> slots;*/

    private bool requireSantitation;
    private int minimalPublicOrder;
    private bool processing;
    private int progressBarMax;
    private int progressBarValue;

    public SeekerViewModel(INavigator navigator, ISettingsStore settingsStore, IProvinceStore provinceStore, ISeekService seekService)
    {
        this.navigator = navigator;
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;
        this.seekService = seekService;

        /*this.slots = this.provinceStore.OldStyleSlots.ToList();*/

        this.requireSantitation = true;
        this.minimalPublicOrder = 1;
        this.processing = false;

        this.SeekCommand = new AsyncRelayCommand(this.Seek, this.SeekEnabled);
        this.PreviousCommand = new RelayCommand(this.Previous, this.PreviousEnabled);
    }

    public bool RequireSantitation
    {
        get => this.requireSantitation;
        set
        {
            if (this.requireSantitation != value)
            {
                this.requireSantitation = value;
                this.OnPropertyChanged(nameof(this.RequireSantitation));
            }
        }
    }

    public int MinimalPublicOrder
    {
        get => this.minimalPublicOrder;
        set
        {
            if (this.minimalPublicOrder != value)
            {
                this.minimalPublicOrder = value;
                this.OnPropertyChanged(nameof(this.MinimalPublicOrder));
            }
        }
    }

    public int ProgressBarMax
    {
        get => this.processing switch { true => this.progressBarMax, false => 0 };
        set
        {
            if (this.progressBarMax != value)
            {
                this.progressBarMax = value;
                this.OnPropertyChanged(nameof(this.ProgressBarMax));
                this.OnPropertyChanged(nameof(this.ProgressBarText));
            }
        }
    }

    public int ProgressBarValue
    {
        get => this.processing switch { true => this.progressBarValue, false => 0 };
        set
        {
            if (this.progressBarValue != value)
            {
                this.progressBarValue = value;
                this.OnPropertyChanged(nameof(this.ProgressBarValue));
                this.OnPropertyChanged(nameof(this.ProgressBarText));
            }
        }
    }

    public string ProgressBarText => this.processing switch { true => $"{this.ProgressBarValue}/{this.ProgressBarMax}", false => string.Empty };

    public AsyncRelayCommand SeekCommand { get; init; }

    public RelayCommand PreviousCommand { get; init; }

    private async Task Seek()
    {
        this.processing = true;
        this.OnPropertyChanged(nameof(this.ProgressBarMax));
        this.OnPropertyChanged(nameof(this.ProgressBarValue));
        this.OnPropertyChanged(nameof(this.ProgressBarText));
        /*if (this.slots.Any())
        {
            await Task.Run(() =>
            {
                this.seekService.Seek(
                    this.settingsStore.Settings,
                    this.settingsStore.Effect,
                    this.settingsStore.Incomes,
                    this.settingsStore.Influences,
                    this.settingsStore.BuildingLibrary,
                    this.faction,
                    this.province,
                    this.slots.ToList(),
                    this.MinimalCondition,
                    x => this.ProgressBarMax = x,
                    x => this.progressBarValue = x);
            });
        }*/

        this.processing = false;
        this.OnPropertyChanged(nameof(this.ProgressBarMax));
        this.OnPropertyChanged(nameof(this.ProgressBarValue));
        this.OnPropertyChanged(nameof(this.ProgressBarText));

        this.Previous();
    }

    private void Previous()
    {
        this.navigator.CurrentViewType = INavigator.ViewType.Province;
    }

    private bool SeekEnabled()
    {
        return !this.processing;
    }

    private bool PreviousEnabled()
    {
        return !this.processing;
    }

    private bool MinimalCondition(ProvinceState state)
    {
        if (state.Food < 0)
        {
            return false;
        }

        if (this.requireSantitation && state.Regions.Any(x => x.Sanitation < 0))
        {
            return false;
        }

        if (state.PublicOrder < this.minimalPublicOrder)
        {
            return false;
        }

        return true;
    }
}