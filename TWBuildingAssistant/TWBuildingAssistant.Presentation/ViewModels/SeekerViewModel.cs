namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Domain.StateModels;
using TWBuildingAssistant.Presentation.Extensions;
using TWBuildingAssistant.Presentation.State;

public class SeekerViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsStore settingsStore;
    private readonly IProvinceStore provinceStore;
    private readonly ISeekService seekService;
    private readonly IConfiguration configuration;

    private bool requireSantitation;
    private int minimalPublicOrder;
    private bool processing;
    private long progressBarMax;
    private long progressBarValue;

    public SeekerViewModel(INavigator navigator, ISettingsStore settingsStore, IProvinceStore provinceStore, ISeekService seekService, IConfiguration configuration)
    {
        this.navigator = navigator;
        this.settingsStore = settingsStore;
        this.provinceStore = provinceStore;
        this.seekService = seekService;
        this.configuration = configuration;

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

    public long ProgressBarMax
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

    public long ProgressBarValue
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

        await Task.Run(() =>
        {
            async Task UpdateProgressMax(long x)
            {
                this.ProgressBarMax = x;
                await Task.CompletedTask;
            }

            async Task UpdateProgressValue(long x)
            {
                this.ProgressBarValue = x;
                await Task.CompletedTask;
            }

            var seekerResults = this.seekService.Seek(
                this.configuration.GetSettings(),
                this.settingsStore.Effect,
                this.settingsStore.BuildingLibrary,
                this.provinceStore.SeekerSettings,
                this.MinimalCondition,
                UpdateProgressMax,
                UpdateProgressValue);

            this.provinceStore.SeekerResults.AddRange(seekerResults);
        });

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