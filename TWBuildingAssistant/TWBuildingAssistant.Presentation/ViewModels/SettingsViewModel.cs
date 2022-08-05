namespace TWBuildingAssistant.Presentation.ViewModels;

using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using TWBuildingAssistant.Model;
using TWBuildingAssistant.Model.Services;

public class SettingsViewModel 
    : ViewModel
{
    private readonly IWorld world;

    private Religion selectedReligion;

    private Province selectedProvince;

    private Faction selectedFaction;

    private int selectedTechnologyTier;

    private bool useAntilegacyTechnologies;

    private int selectedFertilityDrop;

    private Weather selectedWeather;

    private Season selectedSeason;

    private int corruptionRate;

    public SettingsViewModel(IWorld world)
    {
        this.world = world;
        this.Religions = new ObservableCollection<Religion>(this.world.Religions);
        this.selectedReligion = this.Religions[0];
        this.Provinces = new ObservableCollection<Province>(this.world.Provinces);
        this.selectedProvince = this.Provinces[0];
        this.Factions = new ObservableCollection<Faction>(this.world.Factions);
        this.selectedFaction = this.Factions[0];
        this.TechnologyTiers = new ObservableCollection<int>(new int[] { 0, 1, 2, 3, 4 });
        this.selectedTechnologyTier = this.TechnologyTiers[0];
        this.useAntilegacyTechnologies = false;
        this.FertilityDrops = new ObservableCollection<int>(new int[] { 0, -1, -2, -3, -4 });
        this.selectedFertilityDrop = this.FertilityDrops[0];
        this.Weathers = new ObservableCollection<Weather>(this.world.Weathers);
        this.selectedWeather = this.Weathers[0];
        this.Seasons = new ObservableCollection<Season>(this.world.Seasons);
        this.selectedSeason = this.Seasons[0];
        this.corruptionRate = 1;

        this.NextCommand = new RelayCommand(this.Next);
    }

    public event EventHandler<NextTransitionEventArgs> NextTransition;

    public ObservableCollection<Religion> Religions { get; set; }

    public Religion SelectedReligion
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

    public ObservableCollection<Province> Provinces { get; set; }

    public Province SelectedProvince
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

    public ObservableCollection<Faction> Factions { get; set; }

    public Faction SelectedFaction
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

    public ObservableCollection<Weather> Weathers { get; set; }

    public Weather SelectedWeather
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

    public ObservableCollection<Season> Seasons { get; set; }

    public Season SelectedSeason
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

    public RelayCommand NextCommand { get; init; }

    public void Next()
    {
        this.SelectedFaction.FertilityDrop = this.SelectedFertilityDrop;
        this.SelectedFaction.TechnologyTier = this.SelectedTechnologyTier;
        this.SelectedFaction.UseAntilegacyTechnologies = this.UseAntilegacyTechnologies;
        this.SelectedFaction.StateReligion = this.SelectedReligion;
        this.SelectedProvince.Owner = this.SelectedFaction;
        this.SelectedProvince.Weather = this.SelectedWeather;
        this.SelectedProvince.Season = this.SelectedSeason;
        this.SelectedProvince.CorruptionRate = this.CorruptionRate;

        this.NextTransition?.Invoke(this, new NextTransitionEventArgs(this.selectedProvince));
    }

    public class NextTransitionEventArgs : EventArgs
    {
        public NextTransitionEventArgs(Province province)
        {
            this.Province = province;
        }

        public Province Province { get; }
    }
}