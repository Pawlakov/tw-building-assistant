namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Data.FSharp;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Services;
using TWBuildingAssistant.Domain.StateModels;
using TWBuildingAssistant.Presentation.Extensions;
using TWBuildingAssistant.Presentation.State;

public class SettingsViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsService settingsService;
    private readonly ISettingsStore settingsStore;
    private readonly IConfiguration configuration;

    private Settings settings;

    public SettingsViewModel(INavigator navigator, ISettingsService settingsService, ISettingsStore settingsStore, IConfiguration configuration)
    {
        this.navigator = navigator;
        this.settingsService = settingsService;
        this.settingsStore = settingsStore;
        this.configuration = configuration;

        this.Religions = new ObservableCollection<Models.NamedId>(Library.getReligionOptions());
        this.Provinces = new ObservableCollection<Models.NamedId>(Library.getProvinceOptions());
        this.Factions = new ObservableCollection<Models.NamedId>(Library.getFactionOptions());
        this.TechnologyTiers = new ObservableCollection<int>(new int[] { 0, 1, 2, 3, 4 });
        this.FertilityDrops = new ObservableCollection<int>(new int[] { 0, -1, -2, -3, -4 });
        this.Weathers = new ObservableCollection<Models.NamedId>(Library.getWeatherOptions());
        this.Seasons = new ObservableCollection<Models.NamedId>(Library.getSeasonOptions());
        this.Difficulties = new ObservableCollection<Models.NamedId>(Library.getDifficultyOptions());
        this.Taxes = new ObservableCollection<Models.NamedId>(Library.getTaxOptions());

        var settings = this.configuration.GetSettings();
        if (settings == null)
        {
            this.settings = new Settings
            {
                FertilityDrop = this.FertilityDrops[0],
                TechnologyTier = this.TechnologyTiers[0],
                UseAntilegacyTechnologies = false,
                ReligionId = this.Religions[0].Id,
                FactionId = this.Factions[0].Id,
                WeatherId = this.Weathers[0].Id,
                SeasonId = this.Seasons[0].Id,
                CorruptionRate = 1,
                PiracyRate = 1,
                ProvinceId = this.Provinces[0].Id,
                DifficultyId = this.Difficulties[0].Id,
                TaxId = this.Taxes[0].Id,
            };
        }
        else
        {
            this.settings = settings.Value;
        }

        this.NextCommand = new AsyncRelayCommand(this.Next);
    }

    public ObservableCollection<Models.NamedId> Religions { get; set; }

    public Models.NamedId SelectedReligion
    {
        get => this.Religions.FirstOrDefault(x => x.Id == this.settings.ReligionId);
        set
        {
            if (this.settings.ReligionId != value.Id)
            {
                this.settings.ReligionId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedReligion));
            }
        }
    }

    public ObservableCollection<Models.NamedId> Provinces { get; set; }

    public Models.NamedId SelectedProvince
    {
        get => this.Provinces.FirstOrDefault(x => x.Id == this.settings.ProvinceId);
        set
        {
            if (this.settings.ProvinceId != value.Id)
            {
                this.settings.ProvinceId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedProvince));
            }
        }
    }

    public ObservableCollection<Models.NamedId> Factions { get; set; }

    public Models.NamedId SelectedFaction
    {
        get => this.Factions.FirstOrDefault(x => x.Id == this.settings.FactionId);
        set
        {
            if (this.settings.FactionId != value.Id)
            {
                this.settings.FactionId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedFaction));
            }
        }
    }

    public ObservableCollection<int> TechnologyTiers { get; set; }

    public int SelectedTechnologyTier
    {
        get => this.settings.TechnologyTier;
        set
        {
            if (this.settings.TechnologyTier != value)
            {
                this.settings.TechnologyTier = value;
                this.OnPropertyChanged(nameof(this.SelectedTechnologyTier));
            }
        }
    }

    public bool UseAntilegacyTechnologies
    {
        get => this.settings.UseAntilegacyTechnologies;
        set
        {
            if (this.settings.UseAntilegacyTechnologies != value)
            {
                this.settings.UseAntilegacyTechnologies = value;
                this.OnPropertyChanged(nameof(this.UseAntilegacyTechnologies));
            }
        }
    }

    public ObservableCollection<int> FertilityDrops { get; set; }

    public int SelectedFertilityDrop
    {
        get => this.settings.FertilityDrop;
        set
        {
            if (this.settings.FertilityDrop != value)
            {
                this.settings.FertilityDrop = value;
                this.OnPropertyChanged(nameof(this.SelectedFertilityDrop));
            }
        }
    }

    public ObservableCollection<Models.NamedId> Weathers { get; set; }

    public Models.NamedId SelectedWeather
    {
        get => this.Weathers.FirstOrDefault(x => x.Id == this.settings.WeatherId);
        set
        {
            if (this.settings.WeatherId != value.Id)
            {
                this.settings.WeatherId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedWeather));
            }
        }
    }

    public ObservableCollection<Models.NamedId> Seasons { get; set; }

    public Models.NamedId SelectedSeason
    {
        get => this.Seasons.FirstOrDefault(x => x.Id == this.settings.SeasonId);
        set
        {
            if (this.settings.SeasonId != value.Id)
            {
                this.settings.SeasonId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedSeason));
            }
        }
    }

    public ObservableCollection<Models.NamedId> Difficulties { get; set; }

    public Models.NamedId SelectedDifficulty
    {
        get => this.Difficulties.FirstOrDefault(x => x.Id == this.settings.DifficultyId);
        set
        {
            if (this.settings.DifficultyId != value.Id)
            {
                this.settings.DifficultyId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedDifficulty));
            }
        }
    }

    public ObservableCollection<Models.NamedId> Taxes { get; set; }

    public Models.NamedId SelectedTax
    {
        get => this.Taxes.FirstOrDefault(x => x.Id == this.settings.TaxId);
        set
        {
            if (this.settings.TaxId != value.Id)
            {
                this.settings.TaxId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedTax));
            }
        }
    }

    public int CorruptionRate
    {
        get => this.settings.CorruptionRate;
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

            if (this.settings.CorruptionRate != value)
            {
                this.settings.CorruptionRate = value;
                this.OnPropertyChanged(nameof(this.CorruptionRate));
            }
        }
    }

    public int PiracyRate
    {
        get => this.settings.PiracyRate;
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

            if (this.settings.PiracyRate != value)
            {
                this.settings.PiracyRate = value;
                this.OnPropertyChanged(nameof(this.PiracyRate));
            }
        }
    }

    public AsyncRelayCommand NextCommand { get; init; }

    public async Task Next()
    {
        this.configuration.SetSettings(this.settings);

        this.settingsStore.Effect = await this.settingsService.GetStateFromSettings(this.settings);
        this.settingsStore.BuildingLibrary = await this.settingsService.GetBuildingLibrary(this.settings);

        this.navigator.CurrentViewType = INavigator.ViewType.Province;
    }
}