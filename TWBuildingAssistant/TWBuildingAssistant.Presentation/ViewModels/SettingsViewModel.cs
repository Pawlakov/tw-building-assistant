namespace TWBuildingAssistant.Presentation.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Domain.StateModels;
using TWBuildingAssistant.Presentation.State;

public class SettingsViewModel 
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsService settingsService;
    private readonly ISettingsStore settingsStore;

    private NamedId selectedReligion;
    private NamedId selectedProvince;
    private NamedId selectedFaction;
    private int selectedTechnologyTier;
    private bool useAntilegacyTechnologies;
    private int selectedFertilityDrop;
    private NamedId selectedWeather;
    private NamedId selectedSeason;
    private int corruptionRate;

    public SettingsViewModel(INavigator navigator, ISettingsService settingsService, ISettingsStore settingsStore)
    {
        this.navigator = navigator;
        this.settingsService = settingsService;
        this.settingsStore = settingsStore;

        this.Religions = new ObservableCollection<NamedId>(this.settingsService.GetReligionOptions().Result);
        this.Provinces = new ObservableCollection<NamedId>(this.settingsService.GetProvinceOptions().Result);
        this.Factions = new ObservableCollection<NamedId>(this.settingsService.GetFactionOptions().Result);
        this.TechnologyTiers = new ObservableCollection<int>(new int[] { 0, 1, 2, 3, 4 });
        this.FertilityDrops = new ObservableCollection<int>(new int[] { 0, -1, -2, -3, -4 });
        this.Weathers = new ObservableCollection<NamedId>(this.settingsService.GetWeatherOptions().Result);
        this.Seasons = new ObservableCollection<NamedId>(this.settingsService.GetSeasonOptions().Result);

        if (this.settingsStore.Settings == default)
        {
            this.selectedFertilityDrop = this.FertilityDrops[0];
            this.selectedTechnologyTier = this.TechnologyTiers[0];
            this.useAntilegacyTechnologies = false;
            this.selectedReligion = this.Religions[0];
            this.selectedFaction = this.Factions[0];
            this.selectedWeather = this.Weathers[0];
            this.selectedSeason = this.Seasons[0];
            this.corruptionRate = 1;
            this.selectedProvince = this.Provinces[0];
        }
        else
        {
            this.selectedFertilityDrop = this.settingsStore.Settings.FertilityDrop;
            this.selectedTechnologyTier = this.settingsStore.Settings.TechnologyTier;
            this.useAntilegacyTechnologies = this.settingsStore.Settings.UseAntilegacyTechnologies;
            this.selectedReligion = this.Religions.Single(x => x.Id == this.settingsStore.Settings.ReligionId);
            this.selectedFaction = this.Factions.Single(x => x.Id == this.settingsStore.Settings.FactionId);
            this.selectedWeather = this.Weathers.Single(x => x.Id == this.settingsStore.Settings.WeatherId);
            this.selectedSeason = this.Seasons.Single(x => x.Id == this.settingsStore.Settings.SeasonId);
            this.corruptionRate = this.settingsStore.Settings.CorruptionRate;
            this.selectedProvince = this.Provinces.Single(x => x.Id == this.settingsStore.Settings.ProvinceId);
        }

        this.NextCommand = new AsyncRelayCommand(this.Next);
    }

    public ObservableCollection<NamedId> Religions { get; set; }

    public NamedId SelectedReligion
    {
        get => this.selectedReligion;
        set
        {
            if (this.selectedReligion != value)
            {
                this.selectedReligion = value;
                this.OnPropertyChanged(nameof(this.SelectedReligion));
            }
        }
    }

    public ObservableCollection<NamedId> Provinces { get; set; }

    public NamedId SelectedProvince
    {
        get => this.selectedProvince;
        set
        {
            if (this.selectedProvince != value)
            {
                this.selectedProvince = value;
                this.OnPropertyChanged(nameof(this.SelectedProvince));
            }
        }
    }

    public ObservableCollection<NamedId> Factions { get; set; }

    public NamedId SelectedFaction
    {
        get => this.selectedFaction;
        set
        {
            if (this.selectedFaction != value)
            {
                this.selectedFaction = value;
                this.OnPropertyChanged(nameof(this.SelectedFaction));
            }
        }
    }

    public ObservableCollection<int> TechnologyTiers { get; set; }

    public int SelectedTechnologyTier
    {
        get => this.selectedTechnologyTier;
        set
        {
            if (this.selectedTechnologyTier != value)
            {
                this.selectedTechnologyTier = value;
                this.OnPropertyChanged(nameof(this.SelectedTechnologyTier));
            }
        }
    }

    public bool UseAntilegacyTechnologies
    {
        get => this.useAntilegacyTechnologies;
        set
        {
            if (this.useAntilegacyTechnologies != value)
            {
                this.useAntilegacyTechnologies = value;
                this.OnPropertyChanged(nameof(this.UseAntilegacyTechnologies));
            }
        }
    }

    public ObservableCollection<int> FertilityDrops { get; set; }

    public int SelectedFertilityDrop
    {
        get => this.selectedFertilityDrop;
        set
        {
            if (this.selectedFertilityDrop != value)
            {
                this.selectedFertilityDrop = value;
                this.OnPropertyChanged(nameof(this.SelectedFertilityDrop));
            }
        }
    }

    public ObservableCollection<NamedId> Weathers { get; set; }

    public NamedId SelectedWeather
    {
        get => this.selectedWeather;
        set
        {
            if (this.selectedWeather != value)
            {
                this.selectedWeather = value;
                this.OnPropertyChanged(nameof(this.SelectedWeather));
            }
        }
    }

    public ObservableCollection<NamedId> Seasons { get; set; }

    public NamedId SelectedSeason
    {
        get => this.selectedSeason;
        set
        {
            if (this.selectedSeason != value)
            {
                this.selectedSeason = value;
                this.OnPropertyChanged(nameof(this.SelectedSeason));
            }
        }
    }

    public int CorruptionRate
    {
        get => this.corruptionRate;
        set
        {
            if (value > 99)
            {
                value = 99;
            }
            else if (value < 1)
            {
                value = 1;
            }

            if (this.corruptionRate != value)
            {
                this.corruptionRate = value;
                this.OnPropertyChanged(nameof(this.CorruptionRate));
            }
        }
    }

    public AsyncRelayCommand NextCommand { get; init; }

    public async Task Next()
    {
        this.settingsStore.Settings = new Settings(this.selectedProvince.Id, this.SelectedFertilityDrop, this.SelectedTechnologyTier, this.UseAntilegacyTechnologies, this.SelectedReligion.Id, this.SelectedFaction.Id, this.SelectedWeather.Id, this.SelectedSeason.Id, this.CorruptionRate);
        (this.settingsStore.Effect, this.settingsStore.Incomes, this.settingsStore.Influences) = await this.settingsService.GetStateFromSettings(this.settingsStore.Settings);
        this.settingsStore.BuildingLibrary = await this.settingsService.GetBuildingLibrary(this.settingsStore.Settings);

        this.navigator.CurrentViewType = INavigator.ViewType.Province;
    }
}