﻿namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Presentation.Extensions;
using TWBuildingAssistant.Presentation.State;
using static TWBuildingAssistant.Domain.DTOs;
using static TWBuildingAssistant.Domain.Interface;

public class SeekerViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly IProvinceStore provinceStore;
    private readonly IConfiguration configuration;
    private readonly DatabaseContextFactory dbContextFactory;

    private bool requireSantitation;
    private int minimalPublicOrder;
    private bool processing;
    private int progressBarMax;
    private int progressBarValue;

    public SeekerViewModel(INavigator navigator, IProvinceStore provinceStore, IConfiguration configuration, DatabaseContextFactory dbContextFactory)
    {
        this.navigator = navigator;
        this.provinceStore = provinceStore;
        this.configuration = configuration;
        this.dbContextFactory = dbContextFactory;

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

        using (var dbContext = this.dbContextFactory.CreateDbContext())
        {
            await Task.Run(() =>
            {
                async void ResetProgressBar(int x)
                {
                    this.ProgressBarMax = x;
                    this.ProgressBarValue = 0;
                    await Task.CompletedTask;
                }

                async void IncrementProgressBar()
                {
                    this.ProgressBarValue += 1;
                    await Task.CompletedTask;
                }

                var seekerResults = seek(
                    dbContext,
                    this.configuration.GetSettings(),
                    this.provinceStore.SeekerSettings,
                    this.MinimalCondition(),
                    ResetProgressBar,
                    IncrementProgressBar);

                this.provinceStore.SeekerResults.AddRange(seekerResults);
            });
        }

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

    private MinimalConditionDto MinimalCondition()
    {
        return new MinimalConditionDto(this.RequireSantitation, true, this.MinimalPublicOrder);
    }
}