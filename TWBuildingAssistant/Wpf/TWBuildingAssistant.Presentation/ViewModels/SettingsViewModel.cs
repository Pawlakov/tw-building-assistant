namespace TWBuildingAssistant.Presentation.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Presentation.Extensions;
using TWBuildingAssistant.Presentation.State;
using static TWBuildingAssistant.Domain.DTOs;
using static TWBuildingAssistant.Domain.Interface;

public class SettingsViewModel
    : ViewModel
{
    private readonly INavigator navigator;
    private readonly ISettingsStore settingsStore;
    private readonly IConfiguration configuration;

    private Models.Settings settings;

    public SettingsViewModel(INavigator navigator, ISettingsStore settingsStore, IConfiguration configuration)
    {
        this.navigator = navigator;
        this.settingsStore = settingsStore;
        this.configuration = configuration;

        var options = getSettingOptions();
        this.Religions = new ObservableCollection<NamedIdDto>(options.Religions);
        this.Provinces = new ObservableCollection<NamedIdDto>(options.Provinces);
        this.Factions = new ObservableCollection<NamedStringIdDto>(options.Factions);
        this.TechnologyTiers = new ObservableCollection<int>(new int[] { 0, 1, 2, 3, 4 });
        this.FertilityDrops = new ObservableCollection<int>(new int[] { 0, -1, -2, -3, -4 });
        this.Weathers = new ObservableCollection<NamedIdDto>(options.Weathers);
        this.Seasons = new ObservableCollection<NamedIdDto>(options.Seasons);
        this.Difficulties = new ObservableCollection<NamedIdDto>(options.Difficulties);
        this.Taxes = new ObservableCollection<NamedIdDto>(options.Taxes);
        this.PowerLevels = new ObservableCollection<NamedIdDto>(options.PowerLevels);

        var settings = this.configuration.GetSettings();
        if (settings == null)
        {
            this.settings = new Models.Settings
            {
                FertilityDrop = this.FertilityDrops[0],
                TechnologyTier = this.TechnologyTiers[0],
                UseAntilegacyTechnologies = false,
                ReligionId = this.Religions[0].Id,
                FactionId = this.Factions[0].StringId,
                WeatherId = this.Weathers[0].Id,
                SeasonId = this.Seasons[0].Id,
                CorruptionRate = 1,
                PiracyRate = 1,
                ProvinceId = this.Provinces[0].Id,
                DifficultyId = this.Difficulties[0].Id,
                TaxId = this.Taxes[0].Id,
                PowerLevelId = this.PowerLevels[0].Id,
            };
        }
        else
        {
            this.settings = new Models.Settings
            {
                FertilityDrop = settings.FertilityDrop,
                TechnologyTier = settings.TechnologyTier,
                UseAntilegacyTechnologies = settings.UseAntilegacyTechnologies,
                ReligionId = settings.ReligionId,
                FactionId = settings.FactionId,
                WeatherId = settings.WeatherId,
                SeasonId = settings.SeasonId,
                CorruptionRate = settings.CorruptionRate,
                PiracyRate = settings.PiracyRate,
                ProvinceId = settings.ProvinceId,
                DifficultyId = settings.DifficultyId,
                TaxId = settings.TaxId,
                PowerLevelId = settings.PowerLevelId,
            };
        }

        this.NextCommand = new AsyncRelayCommand(this.Next);
    }

    public ObservableCollection<NamedIdDto> Religions { get; set; }

    public NamedIdDto SelectedReligion
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

    public ObservableCollection<NamedIdDto> Provinces { get; set; }

    public NamedIdDto SelectedProvince
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

    public ObservableCollection<NamedStringIdDto> Factions { get; set; }

    public NamedStringIdDto SelectedFaction
    {
        get => this.Factions.FirstOrDefault(x => x.StringId == this.settings.FactionId);
        set
        {
            if (this.settings.FactionId != value.StringId)
            {
                this.settings.FactionId = value.StringId;
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

    public ObservableCollection<NamedIdDto> Weathers { get; set; }

    public NamedIdDto SelectedWeather
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

    public ObservableCollection<NamedIdDto> Seasons { get; set; }

    public NamedIdDto SelectedSeason
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

    public ObservableCollection<NamedIdDto> Difficulties { get; set; }

    public NamedIdDto SelectedDifficulty
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

    public ObservableCollection<NamedIdDto> Taxes { get; set; }

    public NamedIdDto SelectedTax
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

    public ObservableCollection<NamedIdDto> PowerLevels { get; set; }

    public NamedIdDto SelectedPowerLevel
    {
        get => this.PowerLevels.FirstOrDefault(x => x.Id == this.settings.PowerLevelId);
        set
        {
            if (this.settings.PowerLevelId != value.Id)
            {
                this.settings.PowerLevelId = value.Id;
                this.OnPropertyChanged(nameof(this.SelectedPowerLevel));
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
        var settings = new SettingsDto(this.settings.ProvinceId, this.settings.FertilityDrop, this.settings.TechnologyTier, this.settings.UseAntilegacyTechnologies, this.settings.ReligionId, this.settings.FactionId, this.settings.WeatherId, this.settings.SeasonId, this.settings.DifficultyId, this.settings.TaxId, this.settings.PowerLevelId, this.settings.CorruptionRate, this.settings.PiracyRate);
        this.configuration.SetSettings(settings);

        this.settingsStore.BuildingLibrary = getBuildingLibrary(settings);

        this.navigator.CurrentViewType = INavigator.ViewType.Province;
    }
}