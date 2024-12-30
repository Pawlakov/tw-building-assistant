namespace TWBuildingAssistant.Actor.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TWBuildingAssistant.Actor.Extensions;
using TWBuildingAssistant.Actor.State;
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
        var provinceOptions = getProvinceOptions();
        this.Religions = new ObservableCollection<NamedIdDTO>(options.Religions);
        this.Provinces = new ObservableCollection<NamedIdDTO>(provinceOptions.Provinces);
        this.Factions = new ObservableCollection<NamedIdDTO>(options.Factions);
        this.TechnologyTiers = new ObservableCollection<int>(new int[] { 0, 1, 2, 3, 4, 5 });
        this.FertilityDrops = new ObservableCollection<int>(new int[] { 0, -1, -2, -3, -4 });
        this.Weathers = new ObservableCollection<NamedIdDTO>(options.Weathers);
        this.Seasons = new ObservableCollection<NamedIdDTO>(options.Seasons);
        this.Difficulties = new ObservableCollection<NamedIdDTO>(options.Difficulties);
        this.Taxes = new ObservableCollection<NamedIdDTO>(options.Taxes);
        this.PowerLevels = new ObservableCollection<NamedIdDTO>(options.PowerLevels);

        var settings = this.configuration.GetSettings();
        if (settings == null)
        {
            this.settings = new Models.Settings
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

    public ObservableCollection<NamedIdDTO> Religions { get; set; }

    public NamedIdDTO SelectedReligion
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

    public ObservableCollection<NamedIdDTO> Provinces { get; set; }

    public NamedIdDTO SelectedProvince
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

    public ObservableCollection<NamedIdDTO> Factions { get; set; }

    public NamedIdDTO SelectedFaction
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

    public ObservableCollection<NamedIdDTO> Weathers { get; set; }

    public NamedIdDTO SelectedWeather
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

    public ObservableCollection<NamedIdDTO> Seasons { get; set; }

    public NamedIdDTO SelectedSeason
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

    public ObservableCollection<NamedIdDTO> Difficulties { get; set; }

    public NamedIdDTO SelectedDifficulty
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

    public ObservableCollection<NamedIdDTO> Taxes { get; set; }

    public NamedIdDTO SelectedTax
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

    public ObservableCollection<NamedIdDTO> PowerLevels { get; set; }

    public NamedIdDTO SelectedPowerLevel
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
        var settings = new SettingsDTO(this.settings.ProvinceId, this.settings.FertilityDrop, this.settings.TechnologyTier, this.settings.UseAntilegacyTechnologies, this.settings.ReligionId, this.settings.FactionId, this.settings.WeatherId, this.settings.SeasonId, this.settings.DifficultyId, this.settings.TaxId, this.settings.PowerLevelId, this.settings.CorruptionRate, this.settings.PiracyRate);
        this.configuration.SetSettings(settings);

        this.settingsStore.BuildingLibrary = getBuildingLibrary(settings);

        this.navigator.CurrentViewType = INavigator.ViewType.Province;
    }
}
